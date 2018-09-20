# LazyData

A quick data (de)serialization framework for varying data formats in .net with a focus on clean and minimal serialized output for each format, mainly for game development scenarios such as in Unity, MonoGame etc.

[![Build Status][build-status-image]][build-status-url]
[![Nuget Package][nuget-image]][nuget-url]
[![Join Discord Chat][discord-image]][discord-url]

Formats supported

- Xml
- Binary
- Json (via some JSON.Net implementation)
- Bson (built on top of the Json Serializer, *Experimental*)
- Yaml (built on top of the Json Serializer, *Experimental*)

If you are interested in how the outputs would look take a look [here](docs/example-outputs.md)

## Examples 

If you are SUPER lazy you can use the `SuperLazy.Transformer` to get stuff done like a typical serialization framework:

```csharp
var modelIn = new SomeModel();
var data = Transformer.ToJson(modelIn);
var modelOut = Transformer.FromJson(data);
```

You can even lazily transform your data between formats on the fly, which is helpful if you debug in JSON but want to use Binary for production, here is an example of one of the tests:

```csharp
var model = SerializationTestHelper.GeneratePopulatedModel();

var expectdString = Transformer.ToJson(model);
var xml = Transformer.FromJsonToXml(expectdString);
var binary = Transformer.FromXmlToBinary(xml);
var json = Transformer.FromBinaryToJson(binary);

Assert.AreEqual(expectdString, json);
```

## Non Lazy Approach

The static transformer is there for people who dont care about customizing the library and just want to (de)serialize everything however the library was built to be configurable so you can skip the helper `Transformer` class and make the serializers yourself like so:

```csharp
public class SomeClass
{
    public float SomeValue { get; set; }
}

var mappingRegistry = new MappingRegistry(new EverythingTypeMapper());
var serializer = new JsonSerializer(_mappingRegistry);
var output = serializer.Serialize(model);
Console.WriteLine("FileSize: " + output.AsString.Length + " bytes");
Console.WriteLine(output.AsString);
```

### Deserialize stuff
```csharp
var someDataFromAFile = getData();
var dataObject = new DataObject(someDataFromAFile);

var mappingRegistry = new MappingRegistry(new EverythingTypeMapper());
var deserializer = new JsonSerializer(_mappingRegistry);
var model = deserializer.Deserialize<TypeOfModel>(dataObject);
```

You can alternatively use an `DefaultTypeMapper` instead of the `EverythingTypeMapper` which will only attempt to serialize things with attributes on. The allowing you to not need any custom attributes. This is handy for scenarios where you just treat your models as pure data classes.


### Only serialize certain properties
```csharp
[Persist]
public class SomeClass
{
    [PersistData]
    public float SomeValue { get; set; }

    public string NotPersisted { get; set; }
}

var mappingRegistry = new MappingRegistry(new DefaultTypeMapper());
var serializer = new JsonSerializer(_mappingRegistry);
var output = serializer.Serialize(model);

Console.WriteLine("FileSize: " + output.AsString.Length + " bytes");
Console.WriteLine(output.AsString);
```

The `[Persist]` attribute indicates that this should be output in the serialized data, on a class it indicates it is a root object that contains serialized data, on a property it indicates the property should be mapped.

This attribute is used rather than `[Serialize]` because it is due to it being originally meant for ETL processes, if you want to just treat it like a serialization framework you can bypass the need for attributes entirely.

## Docs

Check out the docs directory for docs which go into more detail.

## Tests

There are a suite of unit tests which verify most scenarios, as well as output some performance related stats, it is recommended you take a peek at them if you want to see more advanced scenarios.

## Blurb

This was originally part of the [Persistity library](https://github.com/grofit/persistity) for unity *(which was originally part of [EcsRx library](https://github.com/grofit/ecsrx))*, however was separated out to make more generic and reuseable and although its still meant to be used as a smaller part of a larger process, it has some helpers to allow it to be used just like a basic serializer without much effort.

The original focus of this library was to provide a consistent way to pass data around and convert/transform it between formats and types easily however as the serialization pieces can be used in isolation purely for serialization 


[build-status-image]: https://ci.appveyor.com/api/projects/status/sn2vsn0vfib14emg?svg=true
[build-status-url]: https://ci.appveyor.com/project/grofit/lazydata/branch/master
[nuget-image]: https://img.shields.io/nuget/v/LazyData.svg?style=flat-square
[nuget-url]: https://www.nuget.org/packages/LazyData/
[discord-image]: https://img.shields.io/discord/488609938399297536.svg
[discord-url]: https://discord.gg/bS2rnGz
