using System;
using System.Collections.Generic;

namespace LazyData.Mappings
{
    public class NestedMapping : Mapping
    {
        public Func<object, object> GetValue { get; set; }
        public Action<object, object> SetValue { get; set; }
        public IList<Mapping> InternalMappings { get; }
        public bool IsDynamicType { get; set; }

        public NestedMapping()
        { InternalMappings = new List<Mapping>(); }
    }
}