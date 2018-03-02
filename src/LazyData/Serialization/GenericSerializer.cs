using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LazyData.Exceptions;
using LazyData.Mappings;
using LazyData.Registries;

namespace LazyData.Serialization
{
    public abstract class GenericSerializer<TSerializeState, TDeserializeState> : ISerializer
    {
        public IMappingRegistry MappingRegistry { get; }
        public ISerializationConfiguration<TSerializeState, TDeserializeState> Configuration { get; protected set; }

        protected abstract IPrimitiveHandler<TSerializeState, TDeserializeState> DefaultPrimitiveHandler { get; }

        protected GenericSerializer(IMappingRegistry mappingRegistry, ISerializationConfiguration<TSerializeState, TDeserializeState> configuration = null)
        {
            MappingRegistry = mappingRegistry;
            Configuration = configuration;
        }

        public abstract DataObject Serialize(object data);

        protected abstract TSerializeState GetDynamicTypeState(TSerializeState state, Type type);

        protected abstract void HandleNullData(TSerializeState state);
        protected abstract void HandleNullObject(TSerializeState state);
        protected abstract void AddCountToState(TSerializeState state, int count);

        protected object AttemptGetValue<T>(Mapping mapping, T data, TSerializeState state, bool isObject = true)
        {
            if (data == null)
            {
                if(isObject)
                { HandleNullObject(state); }
                else
                { HandleNullData(state); }

                return null;
            }

            object outputValue = null;

            if (mapping is DictionaryMapping)
            { outputValue = (mapping as DictionaryMapping).GetValue(data); }
            else if (mapping is CollectionMapping)
            { outputValue = (mapping as CollectionMapping).GetValue(data); }
            else if (mapping is PropertyMapping)
            { outputValue = (mapping as PropertyMapping).GetValue(data); }
            else if (mapping is NestedMapping)
            { outputValue = (mapping as NestedMapping).GetValue(data); }

            if (outputValue == null)
            {
                if (isObject)
                { HandleNullObject(state); }
                else
                { HandleNullData(state); }
                return null;
            }

            return outputValue;
        }

        protected virtual void SerializePrimitive(object value, Type type, TSerializeState state)
        {
            if (value == null)
            {
                HandleNullData(state);
                return;
            }

            var isDefaultPrimitive = DefaultPrimitiveHandler.PrimitiveChecker.IsPrimitive(type);
            if (isDefaultPrimitive)
            {
                DefaultPrimitiveHandler.Serialize(state, value, type);
                return;
            }

            var actualType = type;
            var possibleNullableType = MappingRegistry.TypeMapper.TypeAnalyzer.GetNullableType(type);
            if (possibleNullableType != null)
            {
                actualType = possibleNullableType;
                var isNullablePrimitive = DefaultPrimitiveHandler.PrimitiveChecker.IsPrimitive(actualType);
                if (isNullablePrimitive)
                {
                    DefaultPrimitiveHandler.Serialize(state, value, actualType);
                    return;
                }
            }
            
            var matchingHandler = Configuration.PrimitiveHandlers.SingleOrDefault(x => x.PrimitiveChecker.IsPrimitive(actualType));
            if(matchingHandler == null) { throw new NoKnownTypeException(type); }
            matchingHandler.Serialize(state, value, type);
        }

        protected virtual void SerializeProperty<T>(PropertyMapping propertyMapping, T data, TSerializeState state)
        {
            var underlyingValue = AttemptGetValue(propertyMapping, data, state, false);
            if (underlyingValue == null) { return; }
            SerializePrimitive(underlyingValue, propertyMapping.Type, state);
        }

        protected virtual void SerializeDynamicTypeData<T>(T data, TSerializeState state)
        {
            var typeToUse = data.GetType();
            var dynamicTypeState = GetDynamicTypeState(state, typeToUse);
            var isPrimitiveType = MappingRegistry.TypeMapper.TypeAnalyzer.IsPrimitiveType(typeToUse);
            if (isPrimitiveType)
            { SerializePrimitive(data, typeToUse, dynamicTypeState); }
            else
            {
                var mapping = MappingRegistry.GetMappingFor(typeToUse);
                Serialize(mapping.InternalMappings, data, dynamicTypeState);
            }
        }

        protected virtual void SerializeNestedObject<T>(NestedMapping nestedMapping, T data, TSerializeState state)
        {
            var currentData = AttemptGetValue(nestedMapping, data, state);
            if (currentData == null) { return; }

            if (nestedMapping.IsDynamicType)
            {
                SerializeDynamicTypeData(currentData, state);
                return;
            }

            Serialize(nestedMapping.InternalMappings, currentData, state);
        }

        protected virtual void SerializeCollection<T>(CollectionMapping collectionMapping, T data, TSerializeState state)
        {
            var objectValue = AttemptGetValue(collectionMapping, data, state);
            if (objectValue == null) { return; }
            var collectionValue = (objectValue as IList);

            AddCountToState(state, collectionValue.Count);
            for (var i = 0; i < collectionValue.Count; i++)
            {
                var element = collectionValue[i];
                SerializeCollectionElement(collectionMapping, element, state);
            }
        }

        protected virtual void SerializeCollectionElement<T>(CollectionMapping collectionMapping, T element, TSerializeState state)
        {
            if (element == null)
            {
                HandleNullObject(state);
                return;
            }

            if (collectionMapping.IsElementDynamicType)
            {
                SerializeDynamicTypeData(element, state);
                return;
            }
            
            if (collectionMapping.InternalMappings.Count > 0)
            {
                Serialize(collectionMapping.InternalMappings, element, state);
                return;
            }
            SerializePrimitive(element, collectionMapping.CollectionType, state);            
        }

        protected virtual void SerializeDictionary<T>(DictionaryMapping dictionaryMapping, T data, TSerializeState state)
        {
            var objectValue = AttemptGetValue(dictionaryMapping, data, state);
            if(objectValue == null) { return; }
            var dictionaryValue = (objectValue as IDictionary);

            AddCountToState(state, dictionaryValue.Count);
            foreach (var key in dictionaryValue.Keys)
            { SerializeDictionaryKeyValuePair(dictionaryMapping, dictionaryValue, key, state); }
        }

        protected virtual void SerializeDictionaryKeyValuePair(DictionaryMapping dictionaryMapping, IDictionary dictionary, object key, TSerializeState state)
        {
            SerializeDictionaryKey(dictionaryMapping, key, state);
            SerializeDictionaryValue(dictionaryMapping, dictionary[key], state);
        }

        protected virtual void SerializeDictionaryKey(DictionaryMapping dictionaryMapping, object key, TSerializeState state)
        {
            if (dictionaryMapping.IsKeyDynamicType)
            {
                SerializeDynamicTypeData(key, state);
                return;
            }

            if (dictionaryMapping.KeyMappings.Count > 0)
            { Serialize(dictionaryMapping.KeyMappings, key, state); }
            else
            { SerializePrimitive(key, dictionaryMapping.KeyType, state); }
        }

        protected virtual void SerializeDictionaryValue(DictionaryMapping dictionaryMapping, object value, TSerializeState state)
        {
            if (value == null)
            {
                HandleNullData(state);
                return;
            }

            if (dictionaryMapping.IsKeyDynamicType)
            {
                SerializeDynamicTypeData(value, state);
                return;
            }

            if (dictionaryMapping.ValueMappings.Count > 0)
            { Serialize(dictionaryMapping.ValueMappings, value, state); }
            else
            { SerializePrimitive(value, dictionaryMapping.ValueType, state); }
        }

        protected virtual void Serialize<T>(IEnumerable<Mapping> mappings, T data, TSerializeState state)
        {
            foreach (var mapping in mappings)
            { DelegateMappingType(mapping, data, state); }
        }

        protected virtual void DelegateMappingType<T>(Mapping mapping, T data, TSerializeState state)
        {
            if (mapping is PropertyMapping)
            { SerializeProperty((mapping as PropertyMapping), data, state); }
            else if (mapping is NestedMapping)
            { SerializeNestedObject((mapping as NestedMapping), data, state); }
            else if (mapping is DictionaryMapping)
            { SerializeDictionary(mapping as DictionaryMapping, data, state); }
            else
            { SerializeCollection((mapping as CollectionMapping), data, state); }
        }
    }
}