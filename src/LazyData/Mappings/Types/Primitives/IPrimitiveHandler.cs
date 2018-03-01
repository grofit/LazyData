﻿using System;
using System.Collections.Generic;
using LazyData.Mappings.Types.Primitives.Checkers;

namespace LazyData.Mappings.Types.Primitives
{
    public interface IPrimitiveHandler
    {
        IEnumerable<IPrimitiveChecker> PrimitiveChecks { get; }
        void AddPrimitiveCheck(IPrimitiveChecker primitiveCheck);
        bool IsKnownPrimitive(Type type);
    }
}