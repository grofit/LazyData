using System.Collections.Generic;
using System.IO;
using System;
using LazyData.Json;
using LazyData.Json.Handlers;
using LazyData.Mappings.Types;
using LazyData.Registries;
using YamlDotNet.Serialization;

namespace LazyData.Yaml
{
    public class YamlDeserializer : JsonDeserializer, IYamlDeserializer
    {
        // This needs to be injectable but cant until #350 is solved
        // https://github.com/aaubry/YamlDotNet/issues/350
        private readonly Deserializer _yamlDeserializer;
        private readonly Serializer _yamlSerializer;

        public YamlDeserializer(IMappingRegistry mappingRegistry, ITypeCreator typeCreator,
            IEnumerable<IJsonPrimitiveHandler> customPrimitiveHandlers = null) : base(mappingRegistry, typeCreator,
            customPrimitiveHandlers)
        {
            _yamlDeserializer = new DeserializerBuilder().Build();
            _yamlSerializer = new SerializerBuilder().JsonCompatible().Build();
        }

        private DataObject ConvertYamlToJson(DataObject data)
        {
            using (var reader = new StringReader(data.AsString))
            {
                var transientObject = _yamlDeserializer.Deserialize(reader);
                var json = _yamlSerializer.Serialize(transientObject);
                return new DataObject(json);
            }
        }

        public override object Deserialize(DataObject data, Type type = null)
        {
            // This is very un-performant for now, but a foot in the door
            var jsonObject = ConvertYamlToJson(data);
            return base.Deserialize(jsonObject, type);
        }

        public override void DeserializeInto(DataObject data, object existingInstance)
        {
            // This is very unperformant for now, but a foot in the door
            var jsonObject = ConvertYamlToJson(data);
            base.DeserializeInto(data, existingInstance);
        }
    }
}