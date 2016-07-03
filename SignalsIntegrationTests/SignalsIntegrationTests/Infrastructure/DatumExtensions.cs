using System;
using System.Linq;
using Domain;

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

        public static Datum<T>[] WithValueAt<T>(this Datum<T>[] @this, T value, DateTime timestamp)
        {
            @this.Single(datum => datum.Timestamp == timestamp).Value = value;

            return @this;
        }

        public static Datum<T>[] WithGoodQualityValueAt<T>(this Datum<T>[] @this, T value, DateTime timestamp)
        {
            @this.Single(datum => datum.Timestamp == timestamp).Quality = Quality.Good;

            return @this.WithValueAt(value, timestamp);
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
