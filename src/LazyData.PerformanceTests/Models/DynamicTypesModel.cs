using System.Collections.Generic;

namespace LazyData.PerformanceTests.Models
{
    public class DynamicTypesModel
    {
        public object DynamicNestedProperty { get; set; }
        public object DynamicPrimitiveProperty { get; set; }
        public IList<object> DynamicList { get; set; }
        public IDictionary<object, object> DynamicDictionary { get; set; }
    }
}
