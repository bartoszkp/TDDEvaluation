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

        public virtual void CheckGranularityAndDataType(Signal signal) { }

        public virtual Signal GetShadowSignal() { return null; }

        public virtual Quality GetQuality() { return Quality.None; }

        public virtual void SetQuality(Quality quality) { }


        [NHibernateIgnore]
        public abstract Type NativeDataType { get; }
        
    }

    public abstract class MissingValuePolicy<T> : MissingValuePolicyBase
    {
        [NHibernateIgnore]
        public override Type NativeDataType { get { return typeof(T); } }
    }
}