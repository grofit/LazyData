using System;
using System.Collections.Generic;

namespace LazyData.Mappings.Types
{
    public class TypeAnalyzerConfiguration
    {
        public IEnumerable<Type> IgnoredTypes { get; }

        public TypeAnalyzerConfiguration(IEnumerable<Type> ignoredTypes = null)
        {
            IgnoredTypes = ignoredTypes ?? new List<Type>();
        }
    }
}