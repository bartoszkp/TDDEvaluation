﻿using System;
using System.Linq;
using System.Collections.Generic;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public abstract class MissingValuePolicyBase
    {
        public virtual int? Id { get; set; }

        public virtual Signal Signal { get; set; }

        [NHibernateIgnore]
        public abstract Type NativeDataType { get; }
    }

    public abstract class MissingValuePolicy<T> : MissingValuePolicyBase
    {
        [NHibernateIgnore]
        public override Type NativeDataType { get { return typeof(T); } }

        public abstract Datum<T> FillMissingValue(Signal signal, DateTime tmp, IEnumerable<Datum<T>> data, 
            Datum<T> previousDatum = null, Datum<T> nextDatum = null, IEnumerable<Datum<T>> shadowData = null);
    }
}