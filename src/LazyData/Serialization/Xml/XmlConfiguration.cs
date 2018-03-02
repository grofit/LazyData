using System.Collections.Generic;
using System.Xml.Linq;

namespace LazyData.Serialization.Xml
{
    public class XmlConfiguration : ISerializationConfiguration<XElement, XElement>
    {
        public IEnumerable<IPrimitiveHandler<XElement, XElement>> PrimitiveHandlers { get; }

        public XmlConfiguration(IEnumerable<IPrimitiveHandler<XElement, XElement>> typeHandlers = null)
        {
            PrimitiveHandlers = typeHandlers ?? new List<IPrimitiveHandler<XElement, XElement>>();
        }
    }
}