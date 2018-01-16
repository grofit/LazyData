using System.Collections.Generic;

namespace LazyData.Serialization
{
    public class SerializationConfiguration<TSerialize, TDeserialize>
    {
        public IEnumerable<ITypeHandler<TSerialize, TDeserialize>> TypeHandlers { get; set; }

        public static SerializationConfiguration<TSerialize, TDeserialize> Default => new SerializationConfiguration<TSerialize, TDeserialize>()
        {
            TypeHandlers = new ITypeHandler<TSerialize, TDeserialize>[0]
        };
    }
}