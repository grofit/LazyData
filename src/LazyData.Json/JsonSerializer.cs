using System;
using System.Collections;
using System.Collections.Generic;
using LazyData.Extensions;
using LazyData.Json.Handlers;
using LazyData.Mappings;
using LazyData.Registries;
using LazyData.Serialization;
using Newtonsoft.Json.Linq;

namespace LazyData.Json
{
   public class JsonSerializer : GenericSerializer<JToken, JToken>, IJsonSerializer
    {
        public const string TypeField = "@Type";
        public const string DataField = "@Data";
        public const string KeyField = "Key";
        public const string ValueField = "Value";

        public override IPrimitiveHandler<JToken, JToken> DefaultPrimitiveHandler { get; } = new BasicJsonPrimitiveHandler();

        public JsonSerializer(IMappingRegistry mappingRegistry, IEnumerable<IJsonPrimitiveHandler> customPrimitiveHandlers = null) : base(mappingRegistry, customPrimitiveHandlers)
        {}

        protected override void HandleNullData(JToken state)
        { state.Replace(JValue.CreateNull()); }

        protected override void HandleNullObject(JToken state)
        { HandleNullData(state); }

        protected override void AddCountToState(JToken state, int count)
        { }

        protected override JToken GetDynamicTypeState(JToken state, Type type)
        {
            state[TypeField] = type.GetPersistableName();
            return state[DataField] = new JObject();
        }
        
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

            var xmlString = node.ToString();
            return new DataObject(xmlString);
        }

        protected override void Serialize<T>(IEnumerable<Mapping> mappings, T data, JToken state)
        {
            foreach (var mapping in mappings)
            {
                var newElement = new JObject();
                state[mapping.LocalName] = newElement;

                DelegateMappingType(mapping, data, newElement);
            }
        }

        protected override void SerializeCollection<T>(CollectionMapping collectionMapping, T data, JToken state)
        {
            var objectValue = AttemptGetValue(collectionMapping, data, state);
            if (objectValue == null) { return; }
            var collectionValue = (objectValue as IList);

            var jsonArray = new JArray();
            state.Replace(jsonArray);
            for (var i = 0; i < collectionValue.Count; i++)
            {
                var element = collectionValue[i];
                var jsonObject = new JObject();
                jsonArray.Add(jsonObject);
                SerializeCollectionElement(collectionMapping, element, jsonObject);
            }
        }
        
        protected override void SerializeDictionary<T>(DictionaryMapping dictionaryMapping, T data, JToken state)
        {
            var objectValue = AttemptGetValue(dictionaryMapping, data, state);
            if (objectValue == null) { return; }
            var dictionaryValue = (objectValue as IDictionary);

            var jsonArray = new JArray();
            state.Replace(jsonArray);
            foreach (var key in dictionaryValue.Keys)
            {
                var jsonObject = new JObject();
                jsonArray.Add(jsonObject);
                SerializeDictionaryKeyValuePair(dictionaryMapping, dictionaryValue, key, jsonObject);
            }
        }
        
        protected override void SerializeDictionaryKeyValuePair(DictionaryMapping dictionaryMapping, IDictionary dictionary, object key, JToken state)
        {
            var keyElement = new JObject();
            var valueElement = new JObject();
            state[KeyField] = keyElement;
            state[ValueField] = valueElement;

            SerializeDictionaryKey(dictionaryMapping, key, keyElement);
            SerializeDictionaryValue(dictionaryMapping, dictionary[key], valueElement);
        }
    }
}