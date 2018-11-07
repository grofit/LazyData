using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using LazyData.Extensions;
using LazyData.Mappings;
using LazyData.Registries;
using LazyData.Serialization;
using LazyData.Xml.Handlers;

namespace LazyData.Xml
{
    public class XmlSerializer : GenericSerializer<XElement, XElement>, IXmlSerializer
    {
        public const string TypeAttributeName = "Type";
        public const string NullAttributeName = "IsNull";
        public const string CountAttributeName = "Count";
        public const string KeyElementName = "Key";
        public const string ValueElementName = "Value";
        public const string KeyValuePairElementName = "KeyValuePair";
        public const string CollectionElementName = "Collection";
        public const string ContainerElementName = "Container";

        public override IPrimitiveHandler<XElement, XElement> DefaultPrimitiveHandler { get; } = new BasicXmlPrimitiveHandler();

        public XmlSerializer(IMappingRegistry mappingRegistry, IEnumerable<IXmlPrimitiveHandler> customPrimitiveHandlers = null) : base(mappingRegistry, customPrimitiveHandlers)
        {}

        protected override void HandleNullData(XElement state)
        { state.Add(new XAttribute(NullAttributeName, true)); }

        protected override void HandleNullObject(XElement state)
        { HandleNullData(state); }

        protected override void AddCountToState(XElement state, int count)
        { state.Add(new XAttribute(CountAttributeName, count)); }
        
        protected override XElement GetDynamicTypeState(XElement state, Type type)
        {
            var typeAttribute = new XAttribute(TypeAttributeName, type.GetPersistableName());
            state.Add(typeAttribute);
            return state;
        }
        
        public override DataObject Serialize(object data)
        {
            var element = new XElement(ContainerElementName);
            var dataType = data.GetType();
            var typeMapping = MappingRegistry.GetMappingFor(dataType);
            Serialize(typeMapping.InternalMappings, data, element);

            var typeAttribute = new XAttribute(TypeAttributeName, dataType.GetPersistableName());
            element.Add(typeAttribute);
            
            var xmlString = element.ToString();
            return new DataObject(xmlString);
        }

        protected override void Serialize<T>(IEnumerable<Mapping> mappings, T data, XElement state)
        {
            foreach (var mapping in mappings)
            {
                var newElement = new XElement(mapping.LocalName);
                state.Add(newElement);

                DelegateMappingType(mapping, data, newElement);
            }
        }

        protected override void SerializeCollectionElement<T>(CollectionMapping collectionMapping, T element, XElement state)
        {
            var newElement = new XElement(CollectionElementName);
            state.Add(newElement);

            if (element == null)
            {
                HandleNullObject(newElement);
                return;
            }

            if (collectionMapping.IsElementDynamicType)
            {
                SerializeDynamicTypeData(element, newElement);
                return;
            }

            if (collectionMapping.InternalMappings.Count > 0)
            { Serialize(collectionMapping.InternalMappings, element, newElement); }
            else
            { SerializePrimitive(element, collectionMapping.CollectionType, newElement); }
        }

        protected override void SerializeDictionaryKeyValuePair(DictionaryMapping dictionaryMapping, IDictionary dictionary, object key, XElement state)
        {
            var keyElement = new XElement(KeyElementName);
            SerializeDictionaryKey(dictionaryMapping, key, keyElement);

            var valueElement = new XElement(ValueElementName);
            SerializeDictionaryValue(dictionaryMapping, dictionary[key], valueElement);

            var keyValuePairElement = new XElement(KeyValuePairElementName);
            keyValuePairElement.Add(keyElement, valueElement);
            state.Add(keyValuePairElement);
        }
    }
}