using System;
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

        public abstract void FillDatums(ref List<Datum<T>> datumsList, DateTime time, DateTime toExcludedUtc, Signal signal,
            MissingValuePolicy<T> mvp = null, object additionalDatums = null);
    }
}