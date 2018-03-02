using System.Collections.Generic;
using System.Xml.Linq;

namespace LazyData.Serialization.Xml
{
    public class XmlConfiguration : ISerializationConfiguration<XElement, XElement>
    {
        public IEnumerable<IPrimitiveHandler<XElement, XElement>> TypeHandlers { get; }

        public XmlConfiguration(IEnumerable<IPrimitiveHandler<XElement, XElement>> typeHandlers = null)
        {
            TypeHandlers = typeHandlers ?? new List<IPrimitiveHandler<XElement, XElement>>();
        }
    }
}