# Getting Started

If you just want to covnert your models to a given format and read it back in with no need for custom types etc you can just include the `LazyData.SuperLazy` assembly and use the `Transformer` which is a static class which contains methods which expose simple things like `ToJson`, `ToBinary`, `FromXml` etc as well as letting you convert between formats.

However under the hood this helper is just abstracting the composition and initilization of the required components needed for the de/serializers to run.

For example under the hood here is what normal setup would look like:

```csharp
var typeCreator = new TypeCreator();
var typeAnalyzer = new TypeAnalyzer();
var typeMapper = new EverythingTypeMapper(typeAnalyzer);
var mappingRegistry = new MappingRegistry(typeMapper);

var jsonSerializer = new JsonSerializer(_mappingRegistry);
var jsonDeserializer = new JsonDeserializer(_mappingRegistry, _typeCreator);
```

That would setup a JSON serializer and deserializer, and you can setup other de/serializers the same way.

In most cases it is advised you use dependency injection and inject in the serializers however you need.

## Convoluted?

On face value the above may seem convoluted, and you would probably be right, compared to most serialization frameworks you have to setup a load of stuff before you can get on and start serializing stuff.

This is completely true, but the focus of this library is less day to day serialization and more higher level ETL related tasks, it *can* be used as a serialization framework, and if you want to just get up and running just use the `Transformer`, but as part of a bigger process this approach provides a more extensible and configurable way to manage your serialization pipelines.