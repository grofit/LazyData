# Faqs etc

## How performant is it?

Well its pretty fast, there is a basic performance test for you to run yourself if you are interested within the unit test project, but for example:

```
Binary Serializing
Serialized 10000 Entities in 00:00:00.4638837 
    with 0.04638837ms average
Serialized Large Entity with 10000 elements in 00:00:00.3841531
    large Entity Size 2000052bytes
Deserialized Large Entity with 10000 elements in 00:00:00.8442814


Json Serializing
Serialized 10000 Entities in 00:00:01.1072485 
    with 0.11072485 average
Serialized Large Entity with 10000 elements in 00:00:02.0896456
    large Entity Size 14860085bytes
Deserialized Large Entity with 10000 elements in 00:00:02.3295665


Xml Serializing
Serialized 10000 Entities in 00:00:01.0868938 
    with 0.10868938 average
Serialized Large Entity with 10000 elements in 00:00:01.5478223
    large Entity Size 22680128bytes
Deserialized Large Entity with 10000 elements in 00:00:07.6778448
```

It also does not generate too much GC allocations, for binary serialization it is generally a couple of kb GC allocations, deserialization is usually about twice the size due to instantiations.

Generally its fast enough for most use cases, and as it doesn't *require* you to alter your classes in any way, just write your Pocos in a sensible way and dump them out as data.

## Why does it not just do one data format?

It was designed as a part of a bigger library to abstract away data formats, if you want to do JUST Json serialization then just use JSON.Net directly, or one of the other myriad of purely single format focused serializers.

The benefit of this approach is that you can choose how to handle your data, if you want it in JSON to send to a web service, but binary to store as a cache on the file system, no problem just call the appropriate serializer.

Take for example a typical game dev scenario where you have some simple objects representing items, quests, characters etc and you have custom tools that let you alter all these types. Now when you are developing you would probably want to work in a human readable format like Json or Xml, so just use this and output your data as whatever format and load it back in as the same format... but then what about when you go to release... chances are you would want to optimize your files and make them non human readable, so in that case just have a step to output the files as binary. 

You can now abstract away the underlying data format, and dont need to do any fancy manual serialization logic, you just pass in your objects and tell them what format they should be saved/loaded as, and as the formats are all using the same underlying mapping they will all work with the same type without you having to do anything.

## Is this just for Unity?

No no, it was originally created as part of a unity framework but has been split out to be used in any .net platform that supports .net standard 2.0.

## Why is the setup so complicated?

If you cba setting up the depdencies and dont need anything fancy, just include the `SuperLazy` assembly which has a preconfigured static class (yuck) that will just work for you out of the box.

If you do want more control over how things are getting mapped and serialized etc, then you will have to manually set stuff up as explained in the getting started document.

For larger projects its expected that there will be some form of dependency injection system in place or some other mechanism to create objects up front and pass them via IoC into whatever needs them, so the setup may look scary if you were to be doing it every time you want to serialize something, but its assumed you will just set it up once and share the instance everywhere (or if you are too lazy just use the pre made transformer).

## You keep mentioning ETL, what is it and how does it effect me?

ETL stands for `Extract -> Transform -> Load` which is a common term when developing large data reliant applications, the notion is that you take data from somewhere (web service, xml file, database etc) in some various format, then transform the data to some other structure, then load it into somewhere else (database, json file, web service etc).

This was the original reason this library was created, to allow people to create data pipelines with [Persistity](https://github.com/grofit/persistity), you can read more on it all there, but for example if you wanted to save your game data, or upload data to a high score board api, you would probably do the following:

- Get model which contains all needed data
- Convert it to some format required for persistance
- Send payload to some end point

So this could be:

- Take your model -> convert it to JSON -> transform the json slightly -> send to a web service
- Take your model -> convert it to binary -> save on the file system
- Take your model -> convert it to binary -> encrypt it -> save on the file system
- Take your model -> convert it to JSON -> send it to mongodb

I mean the possibilities are endless, but ultimately you can wrap the above logic up in a singular pipeline for a given task, and then share this notion throughout your game, so rather than having to keep hand writing all these various bits all over the place you could do something like:

```csharp
// Same setup as before but we add an encryption processor
var encryptor = new AesEncryptor("some-pass-phrase");
var encryptionProcessor = new EncryptDataProcessor(encryptor);
var writeFileEndpoint = new WriteFileEndpoint("savegame.sav");

// Same as before but we now add the processor into the mix
var saveToBinaryFilePipeline = new PipelineBuilder()
    .SerializeWith(binarySerializer) // <-- That binary serializer would be from this lib
    .ProcessWith(encryptionProcessor)
    .SendTo(writeFileEndpoint)
    .Build();

// Execute the pipeline with your game data
saveToBinaryFilePipeline.Execute(myGameData, SuccessCallback, ErrorCallback);
```
So again pairing this with dependency injection makes an easy way for you to just configure your data persistence needs ahead of time, then inject it into anything that needs it, and lets say you are releasing for a mobile device.

If this stuff sounds cool and you want to learn more then take a look at persistity, which is built to do all of the above.