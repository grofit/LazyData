using Newtonsoft.Json.Linq;

namespace LazyData.Serialization.Json.Handlers
{
    public interface IJsonPrimitiveHandler : IPrimitiveHandler<JToken, JToken>
    {}
}