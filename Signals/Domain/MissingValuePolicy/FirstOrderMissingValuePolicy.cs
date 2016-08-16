﻿using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class FirstOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override IEnumerable<Datum<T>> FillData<T>(Signal signal, IEnumerable<Datum<T>> data, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            return null;
        }
    }
}
