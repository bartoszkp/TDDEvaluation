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

        public abstract IEnumerable<Datum<T>> FillData<T>(Signal signal, IEnumerable<Datum<T>> data, DateTime fromIncludedUtc, DateTime toExcludedUtc);
    }
}