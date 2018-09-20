using LazyData.Serialization;
using Newtonsoft.Json.Linq;

namespace LazyData.Json.Handlers
{
    public interface IJsonPrimitiveHandler : IPrimitiveHandler<JToken, JToken>
    {}
}