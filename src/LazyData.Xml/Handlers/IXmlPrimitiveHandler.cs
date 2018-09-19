using System.Xml.Linq;
using LazyData.Serialization;

namespace LazyData.Xml.Handlers
{
    public interface IXmlPrimitiveHandler : IPrimitiveHandler<XElement, XElement>
    {}
}