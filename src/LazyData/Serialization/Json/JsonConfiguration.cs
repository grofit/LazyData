using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace LazyData.Serialization.Json
{
    public class JsonConfiguration : ISerializationConfiguration<JToken, JToken>
    {
        public IEnumerable<IPrimitiveHandler<JToken, JToken>> TypeHandlers { get; }

        public JsonConfiguration(IEnumerable<IPrimitiveHandler<JToken, JToken>> typeHandlers = null)
        {
            TypeHandlers = typeHandlers ?? new List<IPrimitiveHandler<JToken, JToken>>();
        }
    }
}