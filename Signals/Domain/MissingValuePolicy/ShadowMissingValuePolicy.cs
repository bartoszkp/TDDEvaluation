using Domain.Infrastructure;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Exceptions;



namespace Domain.MissingValuePolicy
{
    public class ShadowMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public virtual Signal ShadowSignal { get; set; }

        public override IEnumerable<Datum<T>> FillData(Signal signal, IEnumerable<Datum<T>> data, DateTime fromIncludedUtc, DateTime toExcludedUtc, ISignalsDataRepository signalsDataRepository)
        {
            List<Domain.Datum<T>> datumSignal = data.ToList<Datum<T>>();
            var datumShadowSignal = signalsDataRepository.GetData<T>(ShadowSignal, fromIncludedUtc, toExcludedUtc).ToList<Datum<T>>();
            List<Domain.Datum<T>> datumsFirst = new List<Datum<T>>();
            while (fromIncludedUtc < toExcludedUtc)
            {
                if (datumSignal.Find(x => x.Timestamp == fromIncludedUtc) == null)
                {
                    if (datumShadowSignal.Find(x => x.Timestamp == fromIncludedUtc) == null)
                    {
                        datumsFirst.Add(new Datum<T>()
                        {
                            Quality = Quality.None,
                            Value = default(T),
                            Timestamp = fromIncludedUtc
                        });
                    }
                    else
                    {
                        datumsFirst.Add(datumShadowSignal.Find(x => x.Timestamp == fromIncludedUtc));
                    }
                }
                else
                {
                    datumsFirst.Add(datumSignal.Find(x => x.Timestamp == fromIncludedUtc));
                }
                fromIncludedUtc = AddingTimespanToDataTime(fromIncludedUtc, signal.Granularity);
            }
            return datumsFirst;
        }

        public override void CheckGranularitiesAndDataTypes(Signal signal)
        {
            if (signal.DataType != ShadowSignal.DataType)
                throw new NotMatchingDataTypesException();
            else if (signal.Granularity != ShadowSignal.Granularity)
                throw new NotMatchingGranularitiesException();
        }

        public override void IsDependencyCycle(Signal signal, IMissingValuePolicyRepository mvpRepository)
        {
            if (signal.Id == ShadowSignal.Id)
                throw new DependencyCycleException();
            var shadowSignalPolicy = mvpRepository.Get(ShadowSignal);
            shadowSignalPolicy.IsDependencyCycle(signal, mvpRepository);
        }

        private DateTime AddingTimespanToDataTime(DateTime addedData, Granularity granularitySignal)
        {
            var checkGranularity = new Dictionary<Granularity, Action>()
            {
                {Granularity.Day, ()=> addedData=addedData.AddDays(1) },
                {Granularity.Hour, ()=> addedData=addedData.AddHours(1) },
                {Granularity.Minute, ()=> addedData=addedData.AddMinutes(1) },
                {Granularity.Month, ()=> addedData=addedData.AddMonths(1) },
                {Granularity.Second, ()=> addedData=addedData.AddSeconds(1) },
                {Granularity.Week, ()=> addedData=addedData.AddDays(7) },
                {Granularity.Year, ()=> addedData=addedData.AddYears(1) }
            };
            checkGranularity[granularitySignal].Invoke();
            return addedData;
        }
    }
}
