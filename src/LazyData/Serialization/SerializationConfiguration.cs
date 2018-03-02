using System.Collections.Generic;

namespace LazyData.Serialization
{
    public class SerializationConfiguration<TSerialize, TDeserialize>
    {
        public IEnumerable<IPrimitiveHandler<TSerialize, TDeserialize>> TypeHandlers { get; set; }

        public static SerializationConfiguration<TSerialize, TDeserialize> Default => new SerializationConfiguration<TSerialize, TDeserialize>()
        {
            TypeHandlers = new IPrimitiveHandler<TSerialize, TDeserialize>[0]
        };
    }
}