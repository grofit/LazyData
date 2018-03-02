using Newtonsoft.Json.Linq;

namespace LazyData.Serialization.Json
{
    public interface IJsonPrimitiveHandler : IPrimitiveHandler<JToken, JToken>
    {}
}