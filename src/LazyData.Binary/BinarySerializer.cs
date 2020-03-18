using System;
using System.Collections.Generic;
using System.IO;
using LazyData.Binary.Handlers;
using LazyData.Extensions;
using LazyData.Registries;
using LazyData.Serialization;

namespace LazyData.Binary
{
    public class BinarySerializer : GenericSerializer<BinaryWriter, BinaryReader>, IBinarySerializer
    {
        public static readonly byte[] NullDataSig = { 141, 141 };
        public static readonly byte[] NullObjectSig = { 141, 229, 141 };

        public override IPrimitiveHandler<BinaryWriter, BinaryReader> DefaultPrimitiveHandler { get; } = new BasicBinaryPrimitiveHandler();

        public BinarySerializer(IMappingRegistry mappingRegistry, IEnumerable<IBinaryPrimitiveHandler> customPrimitiveHandlers = null) : base(mappingRegistry, customPrimitiveHandlers)
        {}

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

        public override DataObject Serialize(object data, bool persistType = false)
        {
            var typeMapping = MappingRegistry.GetMappingFor(data.GetType());
            using (var memoryStream = new MemoryStream())
            using (var binaryWriter = new BinaryWriter(memoryStream))
            {
                binaryWriter.Write(persistType);
                
                if(persistType)
                { binaryWriter.Write(typeMapping.Type.GetPersistableName()); }
                
                Serialize(typeMapping.InternalMappings, data, binaryWriter);
                binaryWriter.Flush();
                memoryStream.Seek(0, SeekOrigin.Begin);

                return new DataObject(memoryStream.ToArray());
            }
        }
    }
}