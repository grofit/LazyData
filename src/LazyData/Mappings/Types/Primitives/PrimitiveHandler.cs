using System;
using System.Collections.Generic;
using LazyData.Mappings.Types.Primitives.Checkers;

namespace LazyData.Mappings.Types.Primitives
{
    public class PrimitiveHandler : IPrimitiveHandler
    {
        private readonly IList<IPrimitiveChecker> _primitiveChecks = new List<IPrimitiveChecker>();
        
        public IEnumerable<IPrimitiveChecker> PrimitiveChecks => _primitiveChecks;

        public void AddPrimitiveCheck(IPrimitiveChecker primitiveCheck)
        { _primitiveChecks.Add(primitiveCheck); }

        public bool IsKnownPrimitive(Type type)
        {
            for (var i = 0; i < _primitiveChecks.Count; i++)
            {
                if(_primitiveChecks[i].IsPrimitive(type))
                { return true; }
            }

            return false;
        }
    }
}