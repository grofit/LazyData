using System;
using System.Collections.Generic;
using System.Linq;
using LazyData.Mappings.Types.Primitives.Checkers;

namespace LazyData.Mappings.Types.Primitives
{
    public class PrimitiveHandler : IPrimitiveHandler
    {
        private readonly IList<IPrimitiveChecker> _primitiveChecks;
        
        public IEnumerable<IPrimitiveChecker> PrimitiveChecks => _primitiveChecks;

        public PrimitiveHandler(params IPrimitiveChecker[] primitiveCheckers)
        { _primitiveChecks = primitiveCheckers.ToList(); }

        public void AddPrimitiveCheck(IPrimitiveChecker primitiveCheck)
        {
            if(_primitiveChecks.Contains(primitiveCheck))
            { return; }

            _primitiveChecks.Add(primitiveCheck);
        }

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