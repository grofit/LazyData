using System;
using System.IO;
using LazyData.Extensions;
using LazyData.Registries;
using LazyData.Serialization.Binary.Handlers;

namespace LazyData.Serialization.Binary
{
    public class BinarySerializer : GenericSerializer<BinaryWriter, BinaryReader>, IBinarySerializer
    {
        public static readonly byte[] NullDataSig = { 141, 141 };
        public static readonly byte[] NullObjectSig = { 141, 229, 141 };

        protected override IPrimitiveHandler<BinaryWriter, BinaryReader> DefaultPrimitiveHandler { get; } = new BasicBinaryPrimitiveHandler();

        public BinarySerializer(IMappingRegistry mappingRegistry, BinaryConfiguration configuration = null) : base(mappingRegistry)
        {
            Configuration = configuration ?? new BinaryConfiguration();
        }

        protected override void HandleNullData(BinaryWriter state)
        { state.Write(NullDataSig); }

        protected override void HandleNullObject(BinaryWriter state)
        { state.Write(NullObjectSig); }

        protected override void AddCountToState(BinaryWriter state, int count)
        { state.Write(count); }

        protected override BinaryWriter GetDynamicTypeState(BinaryWriter state, Type type)
        {
            state.Write(type.GetPersistableName());
            return state;
        }

        public override DataObject Serialize(object data)
        {
            var typeMapping = MappingRegistry.GetMappingFor(data.GetType());
            using (var memoryStream = new MemoryStream())
            using (var binaryWriter = new BinaryWriter(memoryStream))
            {
                binaryWriter.Write(typeMapping.Type.GetPersistableName());
                Serialize(typeMapping.InternalMappings, data, binaryWriter);
                binaryWriter.Flush();
                memoryStream.Seek(0, SeekOrigin.Begin);

                return new DataObject(memoryStream.ToArray());
            }
        }
    }
}