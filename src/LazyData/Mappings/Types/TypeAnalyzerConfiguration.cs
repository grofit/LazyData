using System;
using System.Collections.Generic;

namespace LazyData.Mappings.Types
{
    public class TypeAnalyzerConfiguration
    {
        public IEnumerable<Type> TreatAsPrimitives { get; set; }
        public IEnumerable<Type> IgnoredTypes { get; set; }

        public static TypeAnalyzerConfiguration Default => new TypeAnalyzerConfiguration
        {
            TreatAsPrimitives = new Type[0],
            IgnoredTypes = new Type[0]
        };
    }
}