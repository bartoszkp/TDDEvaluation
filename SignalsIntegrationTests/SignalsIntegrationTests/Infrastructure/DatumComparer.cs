using System;
using System.Collections;
using System.Collections.Generic;
using Domain;

namespace SignalsIntegrationTests.Infrastructure
{
    public class DatumComparer<T> : IComparer
    {
        public int Compare(object x, object y)
        {
            var datum1 = x as Datum<T>;
            var datum2 = y as Datum<T>;

            if (datum1 == null || datum2 == null)
            {
                throw new ArgumentException();
            }

            if (!datum1.Value.Equals(datum2.Value))
            {
                return Comparer<T>.Default.Compare(datum1.Value, datum2.Value);
            }

            if (!datum1.Quality.Equals(datum2.Quality))
            {
                return Comparer<Domain.Quality>.Default.Compare(datum1.Quality, datum2.Quality);
            }

            if (!datum1.Timestamp.Equals(datum2.Timestamp))
            {
                return Comparer<DateTime>.Default.Compare(datum1.Timestamp, datum2.Timestamp);
            }

            return 0;
        }
    }
}
