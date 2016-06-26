﻿using Domain;
using System;
using System.Linq;

namespace SignalsIntegrationTests.Infrastructure
{
    public static class DatumArray<T>
    {
        public static Datum<T>[] WithNoneQualityForRange(DateTime fromIncludedUtc, DateTime toExcludedUtc, Granularity granularity)
        {
            return new Domain.Infrastructure.TimeEnumerator(fromIncludedUtc, toExcludedUtc, granularity)
                .Select(ts => new Datum<T>() { Quality = Quality.None, Timestamp = ts })
                .ToArray();
        }

        public static Datum<T>[] WithSpecificValueAndQualityForRange(T specificValue,
                                                                     Quality specificQuality,
                                                                     DateTime fromIncludedUtc,
                                                                     DateTime toExcludedUtc,
                                                                     Granularity granularity)
        {
            return new Domain.Infrastructure.TimeEnumerator(fromIncludedUtc, toExcludedUtc, granularity)
                .Select(ts => new Datum<T>() { Quality = specificQuality, Value = specificValue, Timestamp = ts })
                .ToArray();
        }
    }
}
