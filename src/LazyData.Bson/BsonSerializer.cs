using System.Collections.Generic;
using System.IO;
using LazyData.Extensions;
using LazyData.Json;
using LazyData.Json.Handlers;
using LazyData.Registries;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;

namespace LazyData.Bson
{
   public class BsonSerializer : JsonSerializer, IBsonSerializer
    {
        public BsonSerializer(IMappingRegistry mappingRegistry, IEnumerable<IJsonPrimitiveHandler> customPrimitiveHandlers = null) : base(mappingRegistry, customPrimitiveHandlers)
        {}

        public override DataObject Serialize(object data, bool persistType = false)
        {
            var node = new JObject();
            var dataType = data.GetType();
            var typeMapping = MappingRegistry.GetMappingFor(dataType);
            Serialize(typeMapping.InternalMappings, data, node);

            if (persistType)
            {
                var typeElement = new JProperty(TypeField, dataType.GetPersistableName());
                node.Add(typeElement);
            }
            
            using(var memoryStream = new MemoryStream())
            using (var bsonWriter = new BsonWriter(memoryStream))
            {
                node.WriteTo(bsonWriter);
                return new DataObject(memoryStream.ToArray());
            }
        }
    }
}