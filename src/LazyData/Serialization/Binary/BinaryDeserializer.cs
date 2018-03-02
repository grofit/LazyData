using System;
using System.Collections.Generic;
using System.IO;
using LazyData.Mappings.Types;
using LazyData.Registries;
using LazyData.Serialization.Binary.Handlers;

namespace LazyData.Serialization.Binary
{
    public class BinaryDeserializer : GenericDeserializer<BinaryWriter, BinaryReader>, IBinaryDeserializer
    {
        public override IPrimitiveHandler<BinaryWriter, BinaryReader> DefaultPrimitiveHandler { get; } = new BasicBinaryPrimitiveHandler();

        public BinaryDeserializer(IMappingRegistry mappingRegistry, ITypeCreator typeCreator, IEnumerable<IBinaryPrimitiveHandler> customPrimitiveHandlers = null) : base(mappingRegistry, typeCreator, customPrimitiveHandlers)
        {}

        protected override bool IsDataNull(BinaryReader reader)
        {
            var currentPosition = reader.BaseStream.Position;

            foreach (var nullByte in BinarySerializer.NullDataSig)
            {
                var readByte = reader.ReadByte();
                if (nullByte != readByte)
                {
                    reader.BaseStream.Position = currentPosition;
                    return false;
                }
            }
            return true;
        }

        protected override bool IsObjectNull(BinaryReader reader)
        {
            var currentPosition = reader.BaseStream.Position;

            foreach (var nullByte in BinarySerializer.NullObjectSig)
            {
                var readByte = reader.ReadByte();
                if (nullByte != readByte)
                {
                    reader.BaseStream.Position = currentPosition;
                    return false;
                }
            }
            return true;
        }
        
        protected override int GetCountFromState(BinaryReader state)
        { return state.ReadInt32(); }

        public override object Deserialize(DataObject data)
        {
            using (var memoryStream = new MemoryStream(data.AsBytes))
            using (var reader = new BinaryReader(memoryStream))
            {
                var typeName = reader.ReadString();
                var type = TypeCreator.LoadType(typeName);
                var typeMapping = MappingRegistry.GetMappingFor(type);
                var instance = Activator.CreateInstance(type);
                Deserialize(typeMapping.InternalMappings, instance, reader);
                return instance;
            }
        }

        public override void DeserializeInto(DataObject data, object existingInstance)
        {
            using (var memoryStream = new MemoryStream(data.AsBytes))
            using (var reader = new BinaryReader(memoryStream))
            {
                var typeName = reader.ReadString();
                var type = TypeCreator.LoadType(typeName);
                var typeMapping = MappingRegistry.GetMappingFor(type);
                Deserialize(typeMapping.InternalMappings, existingInstance, reader);
            }
        }
        
        protected override string GetDynamicTypeNameFromState(BinaryReader state)
        { return state.ReadString(); }

        protected override BinaryReader GetDynamicTypeDataFromState(BinaryReader state)
        { return state; }
    }
}