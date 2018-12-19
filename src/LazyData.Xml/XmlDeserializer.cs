using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using LazyData.Mappings;
using LazyData.Mappings.Types;
using LazyData.Registries;
using LazyData.Serialization;
using LazyData.Xml.Handlers;

namespace LazyData.Xml
{
    public class XmlDeserializer : GenericDeserializer<XElement, XElement>, IXmlDeserializer
    {
        public override IPrimitiveHandler<XElement, XElement> DefaultPrimitiveHandler { get; } = new BasicXmlPrimitiveHandler();

        public XmlDeserializer(IMappingRegistry mappingRegistry, ITypeCreator typeCreator, IEnumerable<IXmlPrimitiveHandler> customPrimitiveHandlers = null) : base(mappingRegistry, typeCreator, customPrimitiveHandlers)
        {}

        protected override bool IsDataNull(XElement state)
        { return state.Attribute(XmlSerializer.NullAttributeName) != null; }

        protected override bool IsObjectNull(XElement state)
        { return IsDataNull(state); }

        protected override int GetCountFromState(XElement state)
        { return int.Parse(state.Attribute(XmlSerializer.CountAttributeName).Value); }
        
        public override object Deserialize(Type type, DataObject data)
        {
            var xDoc = XDocument.Parse(data.AsString);
            var containerElement = xDoc.Element(XmlSerializer.ContainerElementName);
            var typeMapping = MappingRegistry.GetMappingFor(type);

            var instance = Activator.CreateInstance(typeMapping.Type);
            Deserialize(typeMapping.InternalMappings, instance, containerElement);
            return instance;
        }

        public override void DeserializeInto(DataObject data, object existingInstance)
        {
            var xDoc = XDocument.Parse(data.AsString);
            var containerElement = xDoc.Element(XmlSerializer.ContainerElementName);
            var typeMapping = MappingRegistry.GetMappingFor(existingInstance.GetType());
            
            Deserialize(typeMapping.InternalMappings, existingInstance, containerElement);
        }

        protected override void DeserializeCollection<T>(CollectionMapping mapping, T instance, XElement state)
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
                var collectionElement = state.Elements(XmlSerializer.CollectionElementName).ElementAt(i);
                var elementInstance = DeserializeCollectionElement(mapping, collectionElement);

                if (collectionInstance.IsFixedSize)
                { collectionInstance[i] = elementInstance; }
                else
                { collectionInstance.Insert(i, elementInstance); }
            }
        }

        protected override void DeserializeDictionary<T>(DictionaryMapping mapping, T instance, XElement state)
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
                var keyValuePairElement = state.Elements(XmlSerializer.KeyValuePairElementName).ElementAt(i);
                DeserializeDictionaryKeyValuePair(mapping, dictionary, keyValuePairElement);
            }
        }

        protected override void DeserializeDictionaryKeyValuePair(DictionaryMapping mapping, IDictionary dictionary, XElement state)
        {
            var keyElement = state.Element(XmlSerializer.KeyElementName);
            var keyInstance = DeserializeDictionaryKey(mapping, keyElement);
            var valueElement = state.Element(XmlSerializer.ValueElementName);
            var valueInstance = DeserializeDictionaryValue(mapping, valueElement);
            dictionary.Add(keyInstance, valueInstance);
        }

        protected override void Deserialize<T>(IEnumerable<Mapping> mappings, T instance, XElement state)
        {
            foreach (var mapping in mappings)
            {
                var childElement = state.Element(mapping.LocalName);
                DelegateMappingType(mapping, instance, childElement);
            }
        }

        protected override string GetDynamicTypeNameFromState(XElement state)
        { return state.Attribute(XmlSerializer.TypeAttributeName).Value; }

        protected override XElement GetDynamicTypeDataFromState(XElement state)
        { return state; }
    }
}