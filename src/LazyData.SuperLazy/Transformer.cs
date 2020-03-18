using LazyData.Binary;
using LazyData.Json;
using LazyData.Mappings.Mappers;
using LazyData.Mappings.Types;
using LazyData.Registries;
using LazyData.Xml;

namespace LazyData.SuperLazy
{
    public static class Transformer
    {
        private static readonly IMappingRegistry _mappingRegistry;
        private static readonly ITypeAnalyzer _typeAnalyzer;
        private static readonly ITypeCreator _typeCreator;
        private static readonly ITypeMapper _typeMapper;
        
        private static readonly IJsonSerializer _jsonSerializer;
        private static readonly IJsonDeserializer _jsonDeserializer;
        private static readonly IBinarySerializer _binarySerializer;
        private static readonly IBinaryDeserializer _binaryDeserializer;
        private static readonly IXmlSerializer _xmlSerializer;
        private static readonly IXmlDeserializer _xmlDeserializer;

        static Transformer()
        {
            _typeCreator = new TypeCreator();
            _typeAnalyzer = new TypeAnalyzer();
            _typeMapper = new EverythingTypeMapper(_typeAnalyzer);
            _mappingRegistry = new MappingRegistry(_typeMapper);
            
            _jsonSerializer = new JsonSerializer(_mappingRegistry);
            _jsonDeserializer = new JsonDeserializer(_mappingRegistry, _typeCreator);
            _binarySerializer = new BinarySerializer(_mappingRegistry);
            _binaryDeserializer = new BinaryDeserializer(_mappingRegistry, _typeCreator);
            _xmlSerializer = new XmlSerializer(_mappingRegistry);
            _xmlDeserializer = new XmlDeserializer(_mappingRegistry, _typeCreator);
        }
        
        public static byte[] ToBinary<T>(T obj)
        { return _binarySerializer.Serialize(obj).AsBytes; }
        
        public static T FromBinary<T>(byte[] binary) where T : new()
        { return _binaryDeserializer.Deserialize<T>(new DataObject(binary)); }
        
        public static string ToJson<T>(T obj)
        { return _jsonSerializer.Serialize(obj).AsString; }
        
        public static T FromJson<T>(string json) where T : new()
        { return _jsonDeserializer.Deserialize<T>(new DataObject(json)); }
        
        public static string ToXml<T>(T obj)
        { return _xmlSerializer.Serialize(obj).AsString; }
        
        public static T FromXml<T>(string xml) where T : new()
        { return _xmlDeserializer.Deserialize<T>(new DataObject(xml)); }

        public static byte[] FromXmlToBinary<T>(string xml)
        {
            var obj = _xmlDeserializer.Deserialize(new DataObject(xml), typeof(T));
            return _binarySerializer.Serialize(obj).AsBytes;
        }
        
        public static byte[] FromJsonToBinary<T>(string json)
        {
            var obj = _jsonDeserializer.Deserialize(new DataObject(json), typeof(T));
            return _binarySerializer.Serialize(obj).AsBytes;
        }
        
        public static string FromJsonToXml<T>(string json)
        {
            var obj = _jsonDeserializer.Deserialize(new DataObject(json), typeof(T));
            return _xmlSerializer.Serialize(obj).AsString;
        }
        
        public static string FromBinaryToXml<T>(byte[] binary)
        {
            var obj = _binaryDeserializer.Deserialize(new DataObject(binary), typeof(T));
            return _xmlSerializer.Serialize(obj).AsString;
        }
        
        public static string FromXmlToJson<T>(string json)
        {
            var obj = _xmlDeserializer.Deserialize(new DataObject(json), typeof(T));
            return _jsonSerializer.Serialize(obj).AsString;
        }
        
        public static string FromBinaryToJson<T>(byte[] binary)
        {
            var obj = _binaryDeserializer.Deserialize(new DataObject(binary), typeof(T));
            return _jsonSerializer.Serialize(obj).AsString;
        }
    }
}