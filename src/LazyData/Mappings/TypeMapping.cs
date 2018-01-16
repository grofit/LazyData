using System;
using System.Collections.Generic;

namespace LazyData.Mappings
{
    public class TypeMapping
    {
        public string Name { get; set; }
        public Type Type { get; set; }

        public IList<Mapping> InternalMappings { get; }

        public TypeMapping()
        {
            InternalMappings = new List<Mapping>();
        }
    }
}