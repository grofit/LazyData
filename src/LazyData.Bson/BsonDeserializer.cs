using System;
using System.Collections.Generic;
using System.IO;
using LazyData.Json;
using LazyData.Json.Handlers;
using LazyData.Mappings.Types;
using LazyData.Registries;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;

namespace LazyData.Bson
{
    public class BsonDeserializer : JsonDeserializer, IBsonDeserializer
    {
        public BsonDeserializer(IMappingRegistry mappingRegistry, ITypeCreator typeCreator, IEnumerable<IJsonPrimitiveHandler> customPrimitiveHandlers = null) : base(mappingRegistry, typeCreator, customPrimitiveHandlers)
        {}

        public override object Deserialize(Type type, DataObject data)
        {
            JObject jsonData;
            using(var memoryStream = new MemoryStream(data.AsBytes))
            using(var bsonReader = new BsonReader(memoryStream))
            {
                jsonData = (JObject)JToken.ReadFrom(bsonReader);
            }
            
            var typeMapping = MappingRegistry.GetMappingFor(type);
            var instance = Activator.CreateInstance(type);
            
            Deserialize(typeMapping.InternalMappings, instance, jsonData);
            return instance;
        }

        public override void DeserializeInto(DataObject data, object existingInstance)
        {
            JObject jsonData;
            using(var memoryStream = new MemoryStream(data.AsBytes))
            using(var bsonReader = new BsonReader(memoryStream))
            {
                jsonData = (JObject)JToken.ReadFrom(bsonReader);
            }
            
            var typeName = jsonData[JsonSerializer.TypeField].ToString();
            var type = TypeCreator.LoadType(typeName);
            var typeMapping = MappingRegistry.GetMappingFor(type);
            
            Deserialize(typeMapping.InternalMappings, existingInstance, jsonData);
        }
    }
}