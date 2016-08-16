﻿using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class NoneQualityMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override Datum<T> GetDatum()
        {
            return new Datum<T>()
            {
                Quality = Quality.None,
                Value = default(T)
            };
        }
    }
}
