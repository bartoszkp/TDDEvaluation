using System;
using System.Linq;
using Domain;
using Dto.Conversions;

namespace SignalsIntegrationTests.Infrastructure
{
    public static class DatumExtensions
    {
        public static Datum<T>[] WithQuality<T>(this Datum<T>[] @this, Quality quality)
        {
            Array.ForEach(@this, datum => datum.Quality = quality);

            return @this;
        }

        public static Datum<T>[] WithValue<T>(this Datum<T>[] @this, T value)
        {
            Array.ForEach(@this, datum => datum.Value = value);

            return @this;
        }

        public static Datum<T>[] WithValues<T>(this Datum<T>[] @this, T[] values)
        {
            if (@this.Length != values.Length)
            {
                throw new InvalidOperationException();
            }

            for (int i = 0; i < @this.Length; ++i)
            {
                @this[i].Value = values[i];
            }

            return @this;
        }

        public static Datum<T>[] WithQualities<T>(this Datum<T>[] @this, Quality[] qualities)
        {
            if (@this.Length != qualities.Length)
            {
                throw new InvalidOperationException();
            }

            for (int i = 0; i < @this.Length; ++i)
            {
                @this[i].Quality = qualities[i];
            }

            return @this;
        }

        public static Datum<T>[] WithValueAt<T>(this Datum<T>[] @this, T value, DateTime timestamp)
        {
            @this.Single(datum => datum.Timestamp == timestamp).Value = value;

            return @this;
        }

        public static Datum<T>[] WithValueAt<T>(this Datum<T>[] @this, T value, Quality quality, DateTime timestamp)
        {
            @this.Single(datum => datum.Timestamp == timestamp).Value = value;
            @this.Single(datum => datum.Timestamp == timestamp).Quality = quality;

            return @this;
        }

        public static Datum<T>[] WithGoodQualityValueAt<T>(this Datum<T>[] @this, T value, DateTime timestamp)
        {
            return @this.WithValueAt(value, Quality.Good, timestamp);
        }

        public static Datum<T>[] WithSingleGoodQualityValueAt<T>(this Datum<T>[] @this, T value, DateTime timestamp)
        {
            return @this.WithGoodQualityValueAt(value, timestamp);
        }

        public static Datum<T>[] StartingWithGoodQualityValue<T>(this Datum<T>[] @this, T value)
        {
            return @this.StartingWith(value, Quality.Good);
        }

        public static Datum<T>[] StartingWithNoneQuality<T>(this Datum<T>[] @this)
        {
            return @this.StartingWith(default(T), Quality.None);
        }

        public static Datum<T>[] EndingWithGoodQualityValue<T>(this Datum<T>[] @this, T value)
        {
            return @this.EndingWith(value, Quality.Good);
        }

        public static Datum<T>[] StartingWith<T>(this Datum<T>[] @this, T value, Quality quality)
        {
            @this.First().Quality = quality;
            @this.First().Value = value;

            return @this;
        }

        public static Datum<T>[] EndingWith<T>(this Datum<T>[] @this, T value, Quality quality)
        {
            @this.Last().Quality = quality;
            @this.Last().Value = value;

            return @this;
        }

        public static Datum<T>[] FollowedBy<T>(this Datum<T>[] @this, Datum<T>[] tail)
        {
            return @this.Concat(tail).ToArray();
        }
    }
}
