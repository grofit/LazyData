using Newtonsoft.Json.Linq;

namespace LazyData.Serialization.Json
{
    public class JsonConfiguration : SerializationConfiguration<JToken, JToken>
    {
        public static JsonConfiguration Default => new JsonConfiguration
        {
            TypeHandlers = new IPrimitiveHandler<JToken, JToken>[0]
        };
    }
}