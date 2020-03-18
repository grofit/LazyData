using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LazyData.Exceptions;
using LazyData.Mappings;
using LazyData.Mappings.Types;
using LazyData.Registries;

namespace LazyData.Serialization
{
    public abstract class GenericDeserializer<TSerializeState, TDeserializeState> : IDeserializer
    {
        public IMappingRegistry MappingRegistry { get; }
        public ITypeCreator TypeCreator { get; }
        public IEnumerable<IPrimitiveHandler<TSerializeState, TDeserializeState>> CustomPrimitiveHandlers { get; }

        public abstract IPrimitiveHandler<TSerializeState, TDeserializeState> DefaultPrimitiveHandler { get; }

        protected GenericDeserializer(IMappingRegistry mappingRegistry, ITypeCreator typeCreator, IEnumerable<IPrimitiveHandler<TSerializeState, TDeserializeState>> customPrimitiveHandlers)
        {
            MappingRegistry = mappingRegistry;
            TypeCreator = typeCreator;
            CustomPrimitiveHandlers = customPrimitiveHandlers ?? new IPrimitiveHandler<TSerializeState, TDeserializeState>[0];
        }

        public abstract object Deserialize(DataObject data, Type type = null);
        public abstract void DeserializeInto(DataObject data, object existingInstance);

        protected abstract bool IsDataNull(TDeserializeState state);
        protected abstract bool IsObjectNull(TDeserializeState state);
        protected abstract int GetCountFromState(TDeserializeState state);
        
        protected abstract string GetDynamicTypeNameFromState(TDeserializeState state);
        protected abstract TDeserializeState GetDynamicTypeDataFromState(TDeserializeState state);

        public virtual void DeserializeInto<T>(DataObject data, T existingInstance)
        { DeserializeInto(data, (object)existingInstance); }

        public virtual T Deserialize<T>(DataObject data) where T : new()
        { return (T)Deserialize(data, typeof(T)); }

        protected virtual object DeserializeDefaultPrimitive(Type type, TDeserializeState state)
        {
            var matchedHandler = CustomPrimitiveHandlers.FirstOrDefault(x => x.PrimitiveChecker.IsPrimitive(type));
            if (matchedHandler == null) { throw new Exception($"The primitive matched has no handler: {type}"); }
            return matchedHandler.Deserialize(state, type);
        }

        protected IList CreateCollectionFromMapping(CollectionMapping mapping, int count)
        {
            if (mapping.IsArray)
            { return TypeCreator.CreateFixedCollection(mapping.Type, count); }

            if(MappingRegistry.TypeMapper.TypeAnalyzer.IsGenericCollection(mapping.Type))
            { return TypeCreator.CreateList(mapping.CollectionType); }

            return (IList)TypeCreator.Instantiate(mapping.Type);
        }
        
        protected IDictionary CreateDictionaryFromMapping(DictionaryMapping mapping)
        {
            if (MappingRegistry.TypeMapper.TypeAnalyzer.IsGenericDictionary(mapping.Type))
            { return TypeCreator.CreateDictionary(mapping.KeyType, mapping.ValueType); }

            return (IDictionary)TypeCreator.Instantiate(mapping.Type);
        }

        protected virtual void DeserializeProperty<T>(PropertyMapping propertyMapping, T instance, TDeserializeState state)
        {
            if (IsDataNull(state))
            { propertyMapping.SetValue(instance, null); }
            else
            {
                var underlyingValue = DeserializePrimitive(propertyMapping.Type, state);
                propertyMapping.SetValue(instance, underlyingValue);
            }
        }

        protected virtual object DeserializeDynamicTypeData(TDeserializeState state)
        {
            var dynamicTypeName = GetDynamicTypeNameFromState(state);
            var instanceType = TypeCreator.LoadType(dynamicTypeName);
            var dataState = GetDynamicTypeDataFromState(state);

            if (MappingRegistry.TypeMapper.TypeAnalyzer.IsPrimitiveType(instanceType))
            { return DeserializePrimitive(instanceType, dataState); }

            var instance = TypeCreator.Instantiate(instanceType);
            var typeMapping = MappingRegistry.GetMappingFor(instanceType);
            Deserialize(typeMapping.InternalMappings, instance, dataState);

            return instance;
        }

        protected virtual void DeserializeNestedObject<T>(NestedMapping nestedMapping, T instance, TDeserializeState state)
        {
            if (IsObjectNull(state))
            {
                nestedMapping.SetValue(instance, null);
                return;
            }

            if (nestedMapping.IsDynamicType)
            {
                var dynamicInstance = DeserializeDynamicTypeData(state);
                nestedMapping.SetValue(instance, dynamicInstance);
                return;
            }

            var childInstance = TypeCreator.Instantiate(nestedMapping.Type);
            nestedMapping.SetValue(instance, childInstance);
            Deserialize(nestedMapping.InternalMappings, childInstance, state);
        }

        protected virtual void DeserializeCollection<T>(CollectionMapping mapping, T instance, TDeserializeState state)
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
                var elementInstance = DeserializeCollectionElement(mapping, state);

                if (collectionInstance.IsFixedSize)
                { collectionInstance[i] = elementInstance; }
                else
                { collectionInstance.Insert(i, elementInstance); }
            }
        }

        protected virtual object DeserializeCollectionElement(CollectionMapping mapping, TDeserializeState state)
        {
            if (IsObjectNull(state))
            { return null; }

            if (mapping.IsElementDynamicType)
            { return DeserializeDynamicTypeData(state); }

            if (mapping.InternalMappings.Count > 0)
            {
                var elementInstance = TypeCreator.Instantiate(mapping.CollectionType);
                Deserialize(mapping.InternalMappings, elementInstance, state);
                return elementInstance;
            }

            return DeserializePrimitive(mapping.CollectionType, state);
        }

        protected virtual object DeserializePrimitive(Type type, TDeserializeState state)
        {
            if (IsDataNull(state))
            { return null; }

            var isDefaultPrimitive = DefaultPrimitiveHandler.PrimitiveChecker.IsPrimitive(type);
            if (isDefaultPrimitive)
            { return DefaultPrimitiveHandler.Deserialize(state, type); }

            var actualType = type;
            var possibleNullableType = MappingRegistry.TypeMapper.TypeAnalyzer.GetNullableType(type);
            if (possibleNullableType != null)
            {
                actualType = possibleNullableType;
                var isNullablePrimitive = DefaultPrimitiveHandler.PrimitiveChecker.IsPrimitive(actualType);
                if(isNullablePrimitive)
                { return DefaultPrimitiveHandler.Deserialize(state, possibleNullableType); }
            }

            var matchingHandler = CustomPrimitiveHandlers.SingleOrDefault(x => x.PrimitiveChecker.IsPrimitive(actualType));
            if (matchingHandler != null)
            { return matchingHandler.Deserialize(state, actualType); }

            throw new NoKnownTypeException(actualType);
        }

        protected virtual void DeserializeDictionary<T>(DictionaryMapping mapping, T instance, TDeserializeState state)
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
            { DeserializeDictionaryKeyValuePair(mapping, dictionary, state); }
        }

        protected virtual void DeserializeDictionaryKeyValuePair(DictionaryMapping mapping, IDictionary dictionary, TDeserializeState state)
        {
            var keyInstance = DeserializeDictionaryKey(mapping, state);
            var valueInstance = DeserializeDictionaryValue(mapping, state);
            dictionary.Add(keyInstance, valueInstance);
        }

        protected virtual object DeserializeDictionaryKey(DictionaryMapping mapping, TDeserializeState state)
        {
            if (IsDataNull(state))
            { return null; }

            if (mapping.IsKeyDynamicType)
            { return DeserializeDynamicTypeData(state); }

            if (mapping.KeyMappings.Count > 0)
            {
                var keyInstance = TypeCreator.Instantiate(mapping.KeyType);
                Deserialize(mapping.KeyMappings, keyInstance, state);
                return keyInstance;
            }

            return DeserializePrimitive(mapping.KeyType, state);
        }

        protected virtual object DeserializeDictionaryValue(DictionaryMapping mapping, TDeserializeState state)
        {
            if (IsDataNull(state))
            { return null; }

            if (mapping.IsValueDynamicType)
            { return DeserializeDynamicTypeData(state); }

            if (mapping.ValueMappings.Count > 0)
            {
                var valueInstance = Activator.CreateInstance(mapping.ValueType);
                Deserialize(mapping.ValueMappings, valueInstance, state);
                return valueInstance;
            }

            return DeserializePrimitive(mapping.ValueType, state);
        }

        protected virtual void Deserialize<T>(IEnumerable<Mapping> mappings, T instance, TDeserializeState state)
        {
            foreach (var mapping in mappings)
            { DelegateMappingType(mapping, instance, state); }
        }

        protected virtual void DelegateMappingType<T>(Mapping mapping, T instance, TDeserializeState state)
        {
            if (mapping is PropertyMapping)
            { DeserializeProperty((mapping as PropertyMapping), instance, state); }
            else if (mapping is NestedMapping)
            { DeserializeNestedObject((mapping as NestedMapping), instance, state); }
            else if (mapping is DictionaryMapping)
            { DeserializeDictionary(mapping as DictionaryMapping, instance, state); }
            else
            { DeserializeCollection((mapping as CollectionMapping), instance, state); }
        }
    }
}