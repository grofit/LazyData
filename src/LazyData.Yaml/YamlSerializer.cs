using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LazyData.Extensions;
using LazyData.Json;
using LazyData.Json.Handlers;
using LazyData.Registries;
using Newtonsoft.Json.Linq;
using YamlDotNet.Serialization;

namespace LazyData.Yaml
{
   public class YamlSerializer : JsonSerializer, IYamlSerializer
   {
       // This needs to be injectable but cant until #350 is solved
       // https://github.com/aaubry/YamlDotNet/issues/350
       private readonly Serializer _yamlSerializer;

        public YamlSerializer(IMappingRegistry mappingRegistry,
            IEnumerable<IJsonPrimitiveHandler> customPrimitiveHandlers = null) : base(mappingRegistry,
            customPrimitiveHandlers)
        {
            _yamlSerializer = new SerializerBuilder().Build();
        }

       static object ConvertJTokenToObject(JToken token)
       {
           switch (token)
           {
               case JValue _: return ((JValue)token).Value;
               case JArray _: return token.AsEnumerable().Select(ConvertJTokenToObject).ToList();
               case JObject _: return token.AsEnumerable().Cast<JProperty>().ToDictionary(x => x.Name, x => ConvertJTokenToObject(x.Value));
               default: throw new InvalidOperationException("Unexpected token: " + token);
           }
       }
       
        public override DataObject Serialize(object data)
        {
            var node = new JObject();
            var dataType = data.GetType();
            var typeMapping = MappingRegistry.GetMappingFor(dataType);
            Serialize(typeMapping.InternalMappings, data, node);

            var typeElement = new JProperty(TypeField, dataType.GetPersistableName());
            node.Add(typeElement);

            var temporaryObject = ConvertJTokenToObject(node);
            using (var writer = new StringWriter())
            {
                _yamlSerializer.Serialize(writer, temporaryObject);
                var output = writer.ToString();
                return new DataObject(output);
            }
        }
    }
}