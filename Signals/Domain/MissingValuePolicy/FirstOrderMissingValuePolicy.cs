using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Exceptions;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class FirstOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override Datum<T> GetMissingValue(Signal signal, DateTime timestamp, Datum<T> previous = null, Datum<T> next = null, 
            Datum<T> shadowDatum = null)
        {
            if (previous == null || next == null)
                return Datum<T>.CreateNone(signal, timestamp);

            Func<DateTime, DateTime> dateTimeStep = Domain.Services.Implementation.SignalsDomainService.GetTimeStepFunction(signal.Granularity);

            int count = 0;
            int part = 0;
            for (var d = previous.Timestamp; d < next.Timestamp; d = dateTimeStep(d))
            {
                count++;
                if (d == timestamp)
                    part = count - 1;
            }

            var value = GetPart(previous.Value, next.Value, count, part);

            return new Datum<T>
            {
                Quality = previous.Quality.IsLowerQualityThan(next.Quality) ? previous.Quality : next.Quality,
                Value = value,
                Signal = signal,
                Timestamp = timestamp
            };
        }
        private T GetPart<T>(T previous, T next, int count, int part)
        {
            dynamic p = previous;
            dynamic n = next;
            try
            {
                var unit = (n - p) / count;
                return (T)(p + unit * part);
            }
            catch
            {
                throw new InvalidPolicyDataTypeException(typeof(T));
            }
        }
    }
}
