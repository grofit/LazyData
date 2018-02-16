# Gotchas

## 3rd Party Objects

The main gotcha is around the way the types are mapped for transforming, so if you are using the `DefaultTypeMapper` types need to have the `[PersistData]` attribute, which you cannot really insert into 3rd party classes.
 
So it is recommended for this that you make your own models which contain the bits you care about for these scenarios and just copy the data into these transient models before you serialize, if that is not possible then you can use the `EverythingTypeMapper`.

## Ducktyped style data
~~So if you were to have something like a `List<object>` which may contain many different types of object, this would not work without a custom serializer and type mapper, as it would not be able to statically analyse the type and possible types that would be contained.~~

~~As when it came to deserialize it would be unsure as to what type of object each instance would be, so although this library supports lists, dictionaries, arrays etc it is recommended that you make sure your generics are not of a base type wherever possible.~~

~~A possible workaround here is to make a custom class which derives from object and making it a known primitive, so you decide how to serialize this object a single unified way.~~

This is all supported now as `DynamicTypes`

## Cyclic Data

If you have circular references it will blow up. It is advised to not have circular references... it is something that will be looked at, but its trying to do it in a way that will not deteriorate performance too much.

## Data Types

Also there is only support for basic types, so if you end up having complex structs that need transforming in a specific way there will hopefully be a notion of `KnownTypes` where you can provide the transformers some information on how to serialize/deserialize types, but currently it will throw an exception if it doesnt know how to handle a type and cannot nest the object.

## Public Properties

Currently it only supports persisting of public properties, so make sure you factor this into your POCOs.

This was done as in certain frameworks like Unity, they expect certain fields to be public members rathe than properties so this was used to differentiate the expected sort of data.
 
## Migrations

So one discussed point previously has been data migrations, long story short its not a solved problem at the moment, there are a few different ways to solve the problem but this is a problem for another day if people need it.
 
If you don't know what I am on about here is an example:
 
 So you save your `game-1.sav` file which is using version 1 of the model, but now you have an update and have changed the model and its version 2, and has different fields and a data type has changed. So how do you get the old data into the new model format?
 
... well right now you don't, you have to manually map between them yourself, but in the future there may be some way to automagically work with the data in some tertiary format before it becomes a statically typed .net object.

