using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LazyData.Attributes;
using LazyData.Exceptions;
using LazyData.Extensions;
using LazyData.Mappings.Types;

namespace LazyData.Mappings.Mappers
{
    public class DefaultTypeMapper : TypeMapper
    {
        public DefaultTypeMapper(ITypeAnalyzer typeAnalyzer, MappingConfiguration configuration = null) : base(typeAnalyzer, configuration)
        {}

        public override IEnumerable<PropertyInfo> GetPropertiesFor(Type type)
        {
            return base.GetPropertiesFor(type)
                .Where(x => x.HasAttribute<PersistDataAttribute>()&&
                            x.GetSetMethod().IsPublic &&
                            x.GetGetMethod().IsPublic);
        }

        public override TypeMapping GetTypeMappingsFor(Type type)
        {
            if (!type.HasAttribute<PersistAttribute>())
            { throw new TypeNotPersistableException(type); }

            return base.GetTypeMappingsFor(type);
        }
    }
}