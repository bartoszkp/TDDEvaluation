﻿using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class NoneQualityMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public virtual T Value { get; set; }

        public virtual Quality Quality { get; set; }

        public override Datum<T> GetDatumToFill(DateTime timestamp)
        {
            return new Datum<T>()
            {
                Quality = Quality.None,
                Value = default(T),
                Timestamp = timestamp
            };
        }
    }
}
