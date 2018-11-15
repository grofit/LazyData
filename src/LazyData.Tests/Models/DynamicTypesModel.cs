using System.Collections.Generic;
using LazyData.Attributes;

namespace LazyData.Tests.Models
{
    [Persist]
    public class DynamicTypesModel
    {
        [PersistData]
        public object DynamicNestedProperty { get; set; }

        [PersistData]
        public object DynamicPrimitiveProperty { get; set; }

        [PersistData]
        public IList<object> DynamicList { get; set; }
        
        [PersistData]
        public object[] DynamicArray { get; set; }
        
        [PersistData]
        public IEnumerable<object> DynamicEnumerable { get; set; }

        [PersistData]
        public IDictionary<object, object> DynamicDictionary { get; set; }
    }
}