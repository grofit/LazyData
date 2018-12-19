using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LazyData.Json.Handlers;
using LazyData.Mappings;
using LazyData.Mappings.Types;
using LazyData.Registries;
using LazyData.Serialization;
using Newtonsoft.Json.Linq;

namespace LazyData.Json
{
    public class JsonDeserializer : GenericDeserializer<JToken, JToken>, IJsonDeserializer
    {
        public override IPrimitiveHandler<JToken, JToken> DefaultPrimitiveHandler { get; } = new BasicJsonPrimitiveHandler();

        public JsonDeserializer(IMappingRegistry mappingRegistry, ITypeCreator typeCreator, IEnumerable<IJsonPrimitiveHandler> customPrimitiveHandlers = null) : base(mappingRegistry, typeCreator, customPrimitiveHandlers)
        {}

        protected override bool IsDataNull(JToken state)
        {
            if(state == null) { return true; }
            if (state.Type == JTokenType.Null || state.Type == JTokenType.None)
            { return true; }

            return false;
        }

        protected override bool IsObjectNull(JToken state)
        { return IsDataNull(state); }

        protected override string GetDynamicTypeNameFromState(JToken state)
        { return state[JsonSerializer.TypeField].ToString(); }

        protected override JToken GetDynamicTypeDataFromState(JToken state)
        { return state[JsonSerializer.DataField]; }
        
        public override object Deserialize(Type type, DataObject data)
        {
            var jsonData = JObject.Parse(data.AsString);
            var typeMapping = MappingRegistry.GetMappingFor(type);
            var instance = Activator.CreateInstance(type);
            
            Deserialize(typeMapping.InternalMappings, instance, jsonData);
            return instance;
        }

        public override void DeserializeInto(DataObject data, object existingInstance)
        {
            var jsonData = JObject.Parse(data.AsString);
            var typeMapping = MappingRegistry.GetMappingFor(existingInstance.GetType());
            
            Deserialize(typeMapping.InternalMappings, existingInstance, jsonData);
        }

        protected override int GetCountFromState(JToken state)
        { return state.Children().Count(); }

        protected override void Deserialize<T>(IEnumerable<Mapping> mappings, T instance, JToken state)
        {
            foreach (var mapping in mappings)
            {
                var childNode = state.HasValues ? state[mapping.LocalName] : null;
                DelegateMappingType(mapping, instance, childNode);
            }
        }

        protected override void DeserializeCollection<T>(CollectionMapping mapping, T instance, JToken state)
        {
            if (IsObjectNull(state))
            {
                mapping.SetValue(instance, null);
                return;
            }

            var count = GetCountFromState(state);
            var collectionInstance = CreateCollectionFromMapping(mapping, count);
            mapping.SetValue(instance, collectionInstance);

            for (var i = 0; i < count; i++)
            {
                var collectionElement = state[i];
                var elementInstance = DeserializeCollectionElement(mapping, collectionElement);

                if (collectionInstance.IsFixedSize)
                { collectionInstance[i] = elementInstance; }
                else
                { collectionInstance.Insert(i, elementInstance); }
            }
        }

        protected override void DeserializeDictionary<T>(DictionaryMapping mapping, T instance, JToken state)
        {
            if (IsObjectNull(state))
            {
                mapping.SetValue(instance, null);
                return;
            }

            var count = GetCountFromState(state);

            var dictionary = CreateDictionaryFromMapping(mapping);
            mapping.SetValue(instance, dictionary);

            for (var i = 0; i < count; i++)
            {
                var keyValuePairElement = state[i];
                DeserializeDictionaryKeyValuePair(mapping, dictionary, keyValuePairElement);
            }
        }

        protected override void DeserializeDictionaryKeyValuePair(DictionaryMapping mapping, IDictionary dictionary, JToken state)
        {
            var keyElement = state[JsonSerializer.KeyField];
            var keyInstance = DeserializeDictionaryKey(mapping, keyElement);
            var valueElement = state[JsonSerializer.ValueField];
            var valueInstance = DeserializeDictionaryValue(mapping, valueElement);
            dictionary.Add(keyInstance, valueInstance);
        }
    }
}