﻿using System;
using System.Linq;
using Domain;

namespace SignalsIntegrationTests.Infrastructure
{
    public static class DatumArray<T>
    {
        public static Datum<T>[] WithNoneQualityForRange(DateTime fromIncludedUtc, DateTime toExcludedUtc, Granularity granularity)
        {
            return ForRange(fromIncludedUtc, toExcludedUtc, granularity)
                .WithQuality(Quality.None);
        }

        public static Datum<T>[] WithSpecificValueAndQualityForRange(
            T specificValue,
            Quality specificQuality,
            DateTime fromIncludedUtc,
            DateTime toExcludedUtc,
            Granularity granularity)
        {
            return ForRange(fromIncludedUtc, toExcludedUtc, granularity)
                .WithQuality(specificQuality)
                .WithValue(specificValue);
        }

        public static Datum<T>[] WithSpecificValuesAndQualitiesForRange(
            T[] values,
            Quality[] qualities,
            DateTime fromIncludedUtc,
            DateTime toExcludedUtc,
            Granularity granularity)
        {
            return ForRange(fromIncludedUtc, toExcludedUtc, granularity)
                .WithValues(values)
                .WithQualities(qualities);
        }

        public static Datum<T>[] ForRange(DateTime fromIncludedUtc,
                                          DateTime toExcludedUtc,
                                          Granularity granularity)
        {
            return new TimeEnumerator(fromIncludedUtc, toExcludedUtc, granularity)
                .Select(ts => new Datum<T>() { Timestamp = ts })
                .ToArray();
        }
    }
}
