# Flow

There are a few varying bits which do stuff on the process from taking a model and spitting it out in a format.

From the users perspective the flow would start at a serializer but then its just magic from there on, so here is what happens under the hood:

```
ISerializer             <-  This takes the object and gets a mapping from the registry 
     |                      for the type and then runs through the mapping serializing
     |                      and then outputting the data object in the given format.
     |
     |
IMappingRegistry        <-  This contains all mappings for types, if a type has already
     |                      been mapped before then it will not analyze it again, if it 
     |                      has not been mapped it will get it mapped and cache it.
     |  
     |
ITypeMapper             <-  This acts as an entry point to generate a mapping for a given
     |                      type, it will analyze the objects properties and then build
     |                      a mapping tree that represents all the things that need serializing
     |
     |
ITypeAnalyzer           <-  This provides a way for the mapper to analyze the underlying types
                            in a generic way, it provides information such as if something is 
                            a generic, if its a list, what the underlying type is etc.
```

As you can see its not actually that complicated, the main flow is primarily getting a type mapping, then visiting every mapped property and outputting its data in the required format.

Deserializing is the same but it also has an `ITypeCreator` which is tasked with instantiating a type at runtime.

## Type Caching

Because the mappings for each type are cached, it will have to generate the mapping for a type the first time it is required, which can cause an overhead if happening during a game loop or other critical time.

If you want to side step this overhead of generating type mappings at runtime it is recommended you manually call the `mappingRegistry.GetMappingFor<T>()` method before you start your game/app, this way it will build the type mappings ahead of time so when you go to call them at runtime they will already be known about and in memory.