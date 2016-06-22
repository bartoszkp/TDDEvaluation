using Domain;
using System;
using System.Linq;

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

        public static Datum<T>[] WithSingleGoodQualityValueAt<T>(this Datum<T>[] @this, T value, DateTime timestamp)
        {
            @this.Single(datum => datum.Timestamp == timestamp).Quality = Quality.Good;
            @this.Single(datum => datum.Timestamp == timestamp).Value = value;

            return @this;
        }

        public static Datum<T>[] StartingWithGoodQualityValue<T>(this Datum<T>[] @this, T value)
        {
            @this.First().Quality = Quality.Good;
            @this.First().Value = value;

            return @this;
        }

        public static Datum<T>[] EndingWithGoodQualityValue<T>(this Datum<T>[] @this, T value)
        {
            @this.Last().Quality = Quality.Good;
            @this.Last().Value = value;

            return @this;
        }
    }
}
