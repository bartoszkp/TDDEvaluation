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

            switch (result.DataType)
            {
                case DataType.Boolean:
                    this.SetMissingValuePolicy(result, new NoneQualityMissingValuePolicy<bool>());
                    break;
                case DataType.Integer:
                    this.SetMissingValuePolicy(result, new NoneQualityMissingValuePolicy<int>());
                    break;
                case DataType.Double:
                    this.SetMissingValuePolicy(result, new NoneQualityMissingValuePolicy<double>());
                    break;
                case DataType.Decimal:
                    this.SetMissingValuePolicy(result, new NoneQualityMissingValuePolicy<decimal>());
                    break;
                case DataType.String:
                    this.SetMissingValuePolicy(result, new NoneQualityMissingValuePolicy<string>());
                    break;
                default:
                    break;
            }

            return result;
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }


        public Signal Get(Path path)
        { 
            return this.signalsRepository.Get(path);
        }

        public void SetData<T>(IEnumerable<Datum<T>> data)
        {
            this.signalsDataRepository.SetData(data);
        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            List<Datum<T>> result;
            result = this.signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc).OrderBy(d => d.Timestamp).ToList();
            if (result.Count == 0)
            {
                result = new List<Datum<T>>() { new Datum<T>() };
            }

            TimeSpan span = toExcludedUtc.Subtract(fromIncludedUtc);

            var granularity = signal.Granularity;

            int expectedCount;
            TimeSpan maxDifferenceBetweenEachDatum;
            DateTime expectedLastTimeStamp;
            DateTime expectedFirstTimeStamp = fromIncludedUtc;

            switch (granularity)
            {
                case Granularity.Second:
                    expectedCount = span.Seconds;
                    maxDifferenceBetweenEachDatum = new TimeSpan(0, 0, 1);
                    expectedLastTimeStamp = toExcludedUtc - maxDifferenceBetweenEachDatum;
                    break;
                case Granularity.Minute:
                    expectedCount = span.Minutes;
                    maxDifferenceBetweenEachDatum = new TimeSpan(0, 1, 0);
                    expectedLastTimeStamp = toExcludedUtc - maxDifferenceBetweenEachDatum;
                    break;
                case Granularity.Hour:
                    expectedCount = span.Hours;
                    maxDifferenceBetweenEachDatum = new TimeSpan(1, 0, 0);
                    expectedLastTimeStamp = toExcludedUtc - maxDifferenceBetweenEachDatum;
                    break;
                case Granularity.Day:
                    expectedCount = span.Days;
                    maxDifferenceBetweenEachDatum = new TimeSpan(1, 0, 0, 0);
                    expectedLastTimeStamp = toExcludedUtc - maxDifferenceBetweenEachDatum;
                    break;
                case Granularity.Week:
                    expectedCount = span.Days / 7;
                    maxDifferenceBetweenEachDatum = new TimeSpan(7, 0, 0, 0);
                    expectedLastTimeStamp = toExcludedUtc - maxDifferenceBetweenEachDatum;
                    break;
                case Granularity.Month:
                    expectedCount = (toExcludedUtc.Month - fromIncludedUtc.Month) + 12 * (toExcludedUtc.Year - fromIncludedUtc.Year);
                    maxDifferenceBetweenEachDatum = new TimeSpan(31, 0, 0, 0);
                    expectedLastTimeStamp = toExcludedUtc.AddMonths(-1);
                    break;
                case Granularity.Year:
                    expectedCount = toExcludedUtc.Year - fromIncludedUtc.Year;
                    maxDifferenceBetweenEachDatum = new TimeSpan(366, 0, 0, 0);
                    expectedLastTimeStamp = toExcludedUtc.AddYears(-1);
                    break;
                default:
                    expectedCount = 0;
                    maxDifferenceBetweenEachDatum = default(TimeSpan);
                    expectedLastTimeStamp = default(DateTime);
                    break;
            }

            var missingValuePolicy = this.missingValuePolicyRepository.Get(signal);

            if (missingValuePolicy is NoneQualityMissingValuePolicy<T>)
            {
                while (result.Count() < expectedCount)
                {
                    var noneQualityMVP = missingValuePolicy as NoneQualityMissingValuePolicy<T>;

                    if (!result.ElementAt(0).Timestamp.Equals(expectedFirstTimeStamp))
                    {
                        AddSingleMissingDatum<T>(signal: signal,
                            timeStamp: expectedFirstTimeStamp,
                            quality: noneQualityMVP.Quality,
                            value: noneQualityMVP.Value);
                    }

                    if (!result.ElementAt(result.Count() - 1).Timestamp.Equals(expectedLastTimeStamp))
                    {
                        AddSingleMissingDatum<T>(signal: signal,
                            timeStamp: expectedLastTimeStamp,
                            quality: noneQualityMVP.Quality,
                            value: noneQualityMVP.Value);
                    }

                    for (int i = 0; i < result.Count() - 1; i++)
                    {
                        if (result.ElementAt(i + 1).Timestamp.Subtract(result.ElementAt(i).Timestamp) > maxDifferenceBetweenEachDatum)
                        {
                            if (granularity.Equals(Granularity.Month))
                            {
                                AddSingleMissingDatum<T>(signal: signal,
                                    timeStamp: result.ElementAt(i).Timestamp.AddMonths(1),
                                    quality: noneQualityMVP.Quality,
                                    value: noneQualityMVP.Value);
                            }
                            else if (granularity.Equals(Granularity.Year))
                            {
                                AddSingleMissingDatum<T>(signal: signal,
                                    timeStamp: result.ElementAt(i).Timestamp.AddYears(1),
                                    quality: noneQualityMVP.Quality,
                                    value: noneQualityMVP.Value);
                            }
                            else
                            {
                                AddSingleMissingDatum<T>(signal: signal, 
                                    timeStamp: result.ElementAt(i).Timestamp + maxDifferenceBetweenEachDatum, 
                                    quality: noneQualityMVP.Quality, 
                                    value: noneQualityMVP.Value);
                            }
                        }
                    }
                    
                    result = this.signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc).OrderBy(d => d.Timestamp).ToList();
                } 
            }

            return result;
        }

        private void AddSingleMissingDatum<T>(Signal signal, DateTime timeStamp, Quality quality, T value)
        {
            this.signalsDataRepository.SetData<T>(new Datum<T>[]
            {
                new Datum<T>()
                {
                    Signal = signal,
                    Timestamp = timeStamp,
                    Quality = quality,
                    Value = value
                }
            });
        }

        public MissingValuePolicyBase GetMissingValuePolicy(Signal signal)
        {
            var result =  this.missingValuePolicyRepository.Get(signal);

            if (result == null)
                return null;

            return TypeAdapter.Adapt(result, result.GetType(), result.GetType().BaseType)
                as MissingValuePolicy.MissingValuePolicyBase;

        }

        public void SetMissingValuePolicy(Signal signal, MissingValuePolicyBase missingValuePolicy)
        {
            this.missingValuePolicyRepository.Set(signal, missingValuePolicy);
        }

    }
}
