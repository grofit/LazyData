# Type Handling

Under the hood there is a type system which will check what types are being read and will try to intelligently work out how to handle them, at the high level the analyzer will work out if the type being checked is a primitive or complex data type and based on this route it accordingly to other handlers.

So for example a `string` or a `float` would be classed as a primitive type by default, so their values would be output directly into the serialized output, however if I were to have a complex type which cannot be expressed as a single value, like a custom class or a `IList` etc, it would have to step inside the object and analyze what objects are in there.

Eventually the goal here is that the analyzer wants to have traversed the whole object tree until everything can be expressed as primtive types, and as mentioned above, most primitive and basic types this is already done for you as the framework knows how to cope with these types, however there will be cases where you may have something you deem as primitive.

## Primitive Types

A primitive type is deemed as an object that can be represented as a singular serialized value, so even a `DateTime` object can be expressed as a single value which can be reconsumed at point of deserializing. If an object type cannot be expressed as a singular value then it is deemed as  complex and it analyzed further recursively until everything has been analyzed.

So by default there are a lot of basic types handled for you, so for example any of the types (including nullable versions) that conform to below will be handled for free:

```csharp
public virtual bool IsDefaultPrimitiveType(Type type)
{
    return type.IsPrimitive ||
           type == typeof(string) ||
           type == typeof(DateTime) ||
           type == typeof(Guid) ||
           type.IsEnum;
}
```

This is fine for most use cases but there will be times where you will have custom types you want to express as primitive types, and this requires you to do 2 things:

1. Provide a custom `TypeAnalyzerConfiguration` which includes types you wish to explicitly treat as primitives, i.e

```csharp
var typeAnalyzerConfiguration = new TypeAnalyzerConfiguration {
    TreatAsPrimitives = new [] { typeof(MyCustomType1), typeof(MyCustomType2) }
}
var typeAnalyzer = new TypeAnalyzer(typeAnalyzerConfiguration);
```


2. You need to provide each de/serializer a way to de/serialize the types via an `ITypeHandler`, this is passed in with the `SerializationConfiguration`

## Custom Type Handlers

So an `ITypeHandler` is basically the hook that provides the system a way to de/serialize the data types provided, and as each serializer will have a different approach for de/serializing data you need to provide a handler for each type you want to handle.

So for example if you wanted to support `IReactiveProperty<T>` in your code you would need to write a custom handler for it, so here is an example (taken from an EcsRx branch) which covers the de/serialization of this as a primitive type:

```csharp
public class ReactiveBinaryTypeHandler : ITypeHandler<BinaryWriter, BinaryReader>
{
    private readonly BinaryPrimitiveSerializer _primitiveSerializer;
    private readonly BinaryPrimitiveDeserializer _primitiveDeserializer;

    public ReactiveBinaryTypeHandler()
    {
        _primitiveSerializer = new BinaryPrimitiveSerializer();
        _primitiveDeserializer = new BinaryPrimitiveDeserializer();
    }

    public bool MatchesType(Type type)
    {
        return (type.GetGenericTypeDefinition() == typeof(ReactiveProperty<>));
    }

    public void HandleTypeSerialization(BinaryWriter state, object data, Type type)
    {
        var underlyingType = type.GetGenericArguments()[0];
        var underlyingValue = GetValue(data, underlyingType);
        _primitiveSerializer.SerializeDefaultPrimitive(underlyingValue, underlyingType, state);
    }

    public object HandleTypeDeserialization(BinaryReader state, Type type)
    {
        var underlyingType = type.GetGenericArguments()[0];
        var data = _primitiveDeserializer.DeserializeDefaultPrimitive(underlyingType, state);
        var reactiveProperty = Activator.CreateInstance(type); ;
        SetValue(reactiveProperty, underlyingType, data);
        return reactiveProperty;
    }

    private void SetValue(object reactiveProperty, Type genericType, object newValue)
    {
        var typedGeneric = typeof(ReactiveProperty<>).MakeGenericType(genericType);
        var propertyInfo = typedGeneric.GetProperty("Value");
        propertyInfo.SetValue(reactiveProperty, newValue, null);
    }

    private object GetValue(object reactiveProperty, Type genericType)
    {
        var typedGeneric = typeof(ReactiveProperty<>).MakeGenericType(genericType);
        var propertyInfo = typedGeneric.GetProperty("Value");
        return propertyInfo.GetValue(reactiveProperty, null);
    }
}
```

As you can see the `ITypeHandler` interface requires:

```csharp
public interface ITypeHandler<Tin, Tout>
{
    bool MatchesType(Type type);
    void HandleTypeSerialization(Tin state, object data, Type type);
    object HandleTypeDeserialization(Tout state, Type type);
}
```

This means you can do as much or as little in those methods as you require, and there are already existing helper classes for serializing primitives which you can build on top of yourself, so for example if you wanted to support a JSON version of the above you would do:

```csharp
 public class ReactiveJsonTypeHandler : ITypeHandler<JToken, JToken>
{
    private readonly JsonPrimitiveSerializer _primitiveSerializer;
    private readonly JsonPrimitiveDeserializer _primitiveDeserializer;

    public ReactiveJsonTypeHandler()
    {
        _primitiveSerializer = new JsonPrimitiveSerializer();
        _primitiveDeserializer = new JsonPrimitiveDeserializer();
    }

    public bool MatchesType(Type type)
    {
        return (type.GetGenericTypeDefinition() == typeof(ReactiveProperty<>));
    }

    public void HandleTypeSerialization(JToken state, object data, Type type)
    {
        var underlyingType = type.GetGenericArguments()[0];
        var underlyingValue = GetValue(data, underlyingType);
        _primitiveSerializer.SerializeDefaultPrimitive(underlyingValue, underlyingType, state);
    }

    public object HandleTypeDeserialization(JToken state, Type type)
    {
        var underlyingType = type.GetGenericArguments()[0];
        var data = _primitiveDeserializer.DeserializeDefaultPrimitive(underlyingType, state);
        var reactiveProperty = Activator.CreateInstance(type);;
        SetValue(reactiveProperty, underlyingType, data);
        return reactiveProperty;
    }

    private void SetValue(object reactiveProperty, Type genericType, object newValue)
    {
        var typedGeneric = typeof(ReactiveProperty<>).MakeGenericType(genericType);
        var propertyInfo = typedGeneric.GetProperty("Value");
        propertyInfo.SetValue(reactiveProperty, newValue, null);
    }

    private object GetValue(object reactiveProperty, Type genericType)
    {
        var typedGeneric = typeof(ReactiveProperty<>).MakeGenericType(genericType);
        var propertyInfo = typedGeneric.GetProperty("Value");
        return propertyInfo.GetValue(reactiveProperty, null);
    }
}
```

This is still all a bit convoluted, and hopefully going forward this may become a little nicer, but currently it is functional and that is enough for everyone to get on with for now.

OH also here is how you would use one of the above custom `ITypeHandlers`:

```csharp
var jsonConfiguration = new JsonConfiguration
{
    TypeHandlers = new List<ITypeHandler<JToken, JToken>>
    {
        new ReactiveJsonTypeHandler()
    }
};
var jsonSerializer = new JsonSerializer(jsonConfiguration);
```