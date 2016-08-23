using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Exceptions;
using Domain.Infrastructure;
using Domain.MissingValuePolicy;
using Domain.Repositories;
using Mapster;

namespace Domain.Services.Implementation
{
    [UnityRegister]
    public class SignalsDomainService : ISignalsDomainService
    {
        private readonly ISignalsRepository signalsRepository;
        private readonly ISignalsDataRepository signalsDataRepository;
        private readonly IMissingValuePolicyRepository missingValuePolicyRepository;

        public SignalsDomainService(
            ISignalsRepository signalsRepository,
            ISignalsDataRepository signalsDataRepository,
            IMissingValuePolicyRepository missingValuePolicyRepository)
        {
            this.signalsRepository = signalsRepository;
            this.signalsDataRepository = signalsDataRepository;
            this.missingValuePolicyRepository = missingValuePolicyRepository;
        }

        public Signal Add(Signal newSignal)
        {
            if (newSignal.Id.HasValue)
            {
                throw new IdNotNullException();
            }

            var result = this.signalsRepository.Add(newSignal);
            var dataTypeSwitch = new Dictionary<DataType, Action>
            {
                { DataType.Boolean,()=>missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<bool>()) },
                { DataType.Decimal, () =>missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<decimal>())},
                { DataType.Double,() =>missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<double>())},
                { DataType.Integer,()=>missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<int>())},
                { DataType.String, ()=>missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<string>())}
            };
            dataTypeSwitch[newSignal.DataType].Invoke();
            return result;
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public Signal Get(Path newPath)
        {
            if (newPath.Components.Equals(""))
            {
                throw new PathIsEmptyException();
            }

            return this.signalsRepository.Get(newPath);
        }

        public void SetMissingValuePolicy(Domain.Signal signal, MissingValuePolicyBase mvpDomain)
        {
            this.missingValuePolicyRepository.Set(signal, mvpDomain);
        }

        public MissingValuePolicyBase GetMissingValuePolicy(Signal signal)
        {
            var missingValuePolicy = this.missingValuePolicyRepository.Get(signal);
            if (missingValuePolicy == null)
                return null;
            return TypeAdapter.Adapt(missingValuePolicy, missingValuePolicy.GetType(), missingValuePolicy.GetType().BaseType)
                    as MissingValuePolicy.MissingValuePolicyBase;

        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            if (!dateIsValid(signal.Granularity, fromIncludedUtc))
                throw new ArgumentException("Date: "+fromIncludedUtc.ToString()+ "is invalid");

            var returnList = new List<Datum<T>>();
            var datum = new Datum<T>();
            var mvp = GetMissingValuePolicy(signal);
            if (mvp.GetType() == typeof(NoneQualityMissingValuePolicy<T>))
                datum = new Datum<T>()
                {
                    Quality = Quality.None,
                    Value = default(T)
                };
            if (mvp.GetType() == typeof(SpecificValueMissingValuePolicy<T>))
            {
                var svmvp = mvp as SpecificValueMissingValuePolicy<T>;
                datum = new Datum<T>()
                {
                    Quality = svmvp.Quality,
                    Value = svmvp.Value
                };
            }

            var gettingList = this.signalsDataRepository?.GetData<T>(signal, fromIncludedUtc, toExcludedUtc)?.ToArray();
            if (fromIncludedUtc == toExcludedUtc)
            {
                return gettingList;
            }
            
            Datum<T> xx = null;
            while (fromIncludedUtc < toExcludedUtc)
            {
                if (gettingList != null)
                    xx = gettingList.FirstOrDefault(x => x.Timestamp == fromIncludedUtc);
                if (xx == null)
                {
                    if (mvp.GetType() == typeof(ZeroOrderMissingValuePolicy<T>))
                    {
                        if (returnList.Count == 0)
                            datum = new Datum<T>()
                            {
                                Quality = Quality.None,
                                Value = default(T)
                            };
                        else
                            datum = new Datum<T>()
                            {
                                Quality = returnList.Last().Quality,
                                Value = returnList.Last().Value
                            };
                    }
                    returnList.Add(new Datum<T>() { Quality = datum.Quality, Timestamp = fromIncludedUtc, Value = datum.Value, Signal = signal });
                }
                else
                    returnList.Add(xx);
                fromIncludedUtc = AddToDateTime(fromIncludedUtc, signal);
            }
            return returnList;
        }

        public void SetData<T>(Signal signal, IEnumerable<Datum<T>> datum)
        {
            var datumWithSignal = new Datum<T>[datum.Count()];
            int i = 0;
            foreach (var d in datum)
            {
                datumWithSignal[i++] = new Datum<T>()
                {
                    Quality = d.Quality,
                    Timestamp = d.Timestamp,
                    Value = d.Value,
                    Signal = signal
                };
            }
            this.signalsDataRepository.SetData<T>(datumWithSignal);
        }

        public PathEntry GetPathEntry(Path pathDomain)
        {
            List<Domain.Signal> signalList = signalsRepository.GetAllWithPathPrefix(pathDomain).ToList();
            List<Domain.Path> pathList = new List<Path>();
            signalList.ForEach(p => { pathList.Add(p.Path); });
            return new PathEntry(signalList, pathList);


        }
        
        public DateTime AddToDateTime(DateTime date, Signal signal)
        {
            var addTimeSpan = new Dictionary<Granularity, Action>
                {
                    {Granularity.Day,() => date = date.AddDays(1)},
                    {Granularity.Hour,() => date = date.AddHours(1)},
                    {Granularity.Minute,() => date = date.AddMinutes(1)},
                    {Granularity.Month,() => date = date.AddMonths(1)},
                    {Granularity.Second,() => date = date.AddSeconds(1)},
                    {Granularity.Week,() => date = date.AddDays(7)},
                    {Granularity.Year,() => date = date.AddYears(1)}
                };
            addTimeSpan[signal.Granularity].Invoke();
            return date;
        }

        private bool dateIsValid (Granularity granularity, DateTime date)
        {
            DateTime correctDate = new DateTime();
            switch (granularity)
            {
                case (Granularity.Year):
                    correctDate = new DateTime(date.Year, 1, 1);
                    break;

                case (Granularity.Month):
                    correctDate = new DateTime(date.Year, date.Month, 1);
                    break;

                case (Granularity.Week):
                    if (date.DayOfWeek != DayOfWeek.Monday)
                        return false;
                    correctDate = new DateTime(date.Year, date.Month, date.Day);
                    break;

                case (Granularity.Day):
                    correctDate = new DateTime(date.Year, date.Month, date.Day);
                    break;

                case (Granularity.Hour):
                    correctDate = new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0);
                    break;

                case (Granularity.Minute):
                    correctDate = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0);
                    break;

                case (Granularity.Second):
                    correctDate = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
                    break;

                default:
                    throw new NotSupportedException("Granularity: " + granularity.ToString() + " is not supported");
            }
            return date.Ticks == correctDate.Ticks;
        }

    }
}
