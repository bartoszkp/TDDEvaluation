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
                throw new IdNotNullException();

            var signal = signalsRepository.Add(newSignal);
            SetDefaultMissingValuePolicy(signal);
            
            return signal;
        }

        public Signal GetByPath(Path domainPath)
        {
            return signalsRepository.Get(domainPath);
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        private void AddToDateTime(ref DateTime dateTime, Granularity granularity)
        {
            switch (granularity)
            {
                case Granularity.Second:
                    dateTime = dateTime.AddSeconds(1);
                    break;
                case Granularity.Minute:
                    dateTime = dateTime.AddMinutes(1);
                    break;
                case Granularity.Hour:
                    dateTime = dateTime.AddHours(1);
                    break;
                case Granularity.Day:
                    dateTime = dateTime.AddDays(1);
                    break;
                case Granularity.Week:
                    dateTime = dateTime.AddDays(7);
                    break;
                case Granularity.Month:
                    dateTime = dateTime.AddMonths(1);
                    break;
                case Granularity.Year:
                    dateTime = dateTime.AddYears(1);
                    break;
            }
        }

        public void SetData<T>(int signalId, IEnumerable<Datum<T>> data)
        {
            var signal = signalsRepository.Get(signalId);
            var dataDomain = data.ToArray();

            for (int i = 0; i < dataDomain.Count(); i++)
            {
                CheckTimestamp(dataDomain[i].Timestamp, signal.Granularity);
                dataDomain.ElementAt(i).Signal = signal;
            }

            signalsDataRepository.SetData(dataDomain);
        }

        public IEnumerable<Datum<T>> GetData<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var signal = signalsRepository.Get(signalId);

            CheckTimestamp(fromIncludedUtc, signal.Granularity);
            CheckTimestamp(toExcludedUtc, signal.Granularity);
            
            var sortedData = signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc)
                .OrderBy(d => d.Timestamp)
                .ToList();

            var policy = GetMissingValuePolicy(signal.Id.Value) as MissingValuePolicy<T>;
            var index = 0;

            if (fromIncludedUtc == toExcludedUtc) toExcludedUtc = toExcludedUtc.AddTicks(1);
            
            var dataBeforeRequest = signalsDataRepository.GetDataOlderThan<T>(signal, fromIncludedUtc, 1).LastOrDefault();

            if (policy is FirstOrderMissingValuePolicy<T>) sortedData = FirstOrderMissingValuePolicyFunction<T>(fromIncludedUtc, toExcludedUtc, sortedData, signal, policy);
            else
            {
                for (DateTime dt = fromIncludedUtc; dt < toExcludedUtc; AddToDateTime(ref dt, signal.Granularity), index++)
                    if (sortedData.FirstOrDefault(d => d.Timestamp == dt) == null)
                    {
                        if (dataBeforeRequest == null)
                        {
                            sortedData.Insert(index, policy.GetMissingDatum(sortedData, dt));
                        }
                        else
                        {
                            dataBeforeRequest.Timestamp = dt;
                            sortedData.Insert(index, dataBeforeRequest);
                            dataBeforeRequest = null;
                        }
                    }
            }


            return sortedData.Where(x => x.Timestamp >= fromIncludedUtc && x.Timestamp < toExcludedUtc).ToList();
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicyBase domainMvp)
        {
            Signal foundSignal = GetById(signalId);
            if (foundSignal == null)
                throw new SignalNotExistException();
                   
            missingValuePolicyRepository.Set(foundSignal, domainMvp);
        }

        public MissingValuePolicyBase GetMissingValuePolicy(int signalId)
        {
            Signal foundSignal = GetById(signalId);
            if (foundSignal == null)
                throw new SignalNotExistException();

            var mvp = missingValuePolicyRepository.Get(foundSignal);

            if (mvp == null)
                return null;

            return TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType)
                as MissingValuePolicyBase;
        }

        public PathEntry GetAllWithPathPrefix(Path prefix)
        {
            var signals = signalsRepository.GetAllWithPathPrefix(prefix);
            var signalList = new List<Signal>();
            var subPathList = new List<Path>();
            var prefixCnt = prefix.Components.Count();

            foreach(var signal in signals)
            {
                var signalCnt = signal.Path.Components.Count();

                if (signalCnt == prefixCnt + 1)
                    signalList.Add(signal);
                else if (signalCnt > prefixCnt)
                    subPathList.Add(CreatePath(signal.Path.Components.Take(prefixCnt + 1)));
            }

            return new PathEntry(signalList, subPathList.Distinct()); 
        }

        public Type GetSignalType(int signalId)
        {
            var signal = signalsRepository.Get(signalId);

            if (signal == null) throw new SignalNotExistException();

            return DataTypeUtils.GetNativeType(signal.DataType);
        }

        private List<Datum<T>> FirstOrderMissingValuePolicyFunction<T>(DateTime fromIncludedUtc, DateTime toExcludedUtc, List<Datum<T>> data, Signal signal, MissingValuePolicy.MissingValuePolicy<T> policy )
        {
            int index = 0;
            var tempFromIncludeUtc = fromIncludedUtc;
            Quality quality;
            int length;
            dynamic value;
            Datum<T> leftNeighbor = signalsDataRepository.GetDataOlderThan<T>(signal, fromIncludedUtc, 1).LastOrDefault();
            Datum<T> rightNeighbor;

            if (leftNeighbor != null) tempFromIncludeUtc = leftNeighbor.Timestamp;
            for (DateTime dt = tempFromIncludeUtc; dt < toExcludedUtc; AddToDateTime(ref dt, signal.Granularity), index++)
            {
                rightNeighbor = signalsDataRepository.GetDataNewerThan<T>(signal, dt, 1).FirstOrDefault();

                if (data.FirstOrDefault(d => d.Timestamp == dt) == null)
                {
                    if (leftNeighbor != null && rightNeighbor != null)
                    {
                        if (rightNeighbor.Timestamp == dt)
                        {
                            data.Insert(index, new Datum<T>() { Id = rightNeighbor.Id, Quality = rightNeighbor.Quality, Signal = rightNeighbor.Signal, Timestamp = dt, Value = rightNeighbor.Value });
                            continue;
                        }

                        length = LenghtBetweenDatums(rightNeighbor, leftNeighbor, signal.Granularity);
                        value = (dynamic)leftNeighbor.Value + (((dynamic)rightNeighbor.Value - (dynamic)leftNeighbor.Value) / (length));
                        quality = worstQuality(leftNeighbor.Quality, rightNeighbor.Quality);

                        data.Insert(index, new Datum<T>() { Quality = quality, Timestamp = dt, Value = value });
                        leftNeighbor = data.FirstOrDefault(d => d.Timestamp == dt);
                    }
                    else
                    {
                        data.Insert(index, policy.GetMissingDatum(data, dt));
                    }
                }
                else
                {
                    leftNeighbor = data.FirstOrDefault(d => d.Timestamp == dt);
                }

            }
            return data;
        }

        private int LenghtBetweenDatums<T>(Datum<T> datum1, Datum<T> datum2, Granularity granuality)
        {
            if (granuality == Granularity.Second) return (int)(datum1.Timestamp - datum2.Timestamp).TotalSeconds;
            if (granuality == Granularity.Minute) return (int)(datum1.Timestamp - datum2.Timestamp).TotalMinutes;
            if (granuality == Granularity.Hour) return (int)(datum1.Timestamp - datum2.Timestamp).TotalHours;
            if (granuality == Granularity.Day) return (int)(datum1.Timestamp - datum2.Timestamp).TotalDays;
            if (granuality == Granularity.Week) return (int)(datum1.Timestamp - datum2.Timestamp).TotalDays;
            if (granuality == Granularity.Month) return ((datum1.Timestamp.Year - datum2.Timestamp.Year) * 12) + datum1.Timestamp.Month - datum2.Timestamp.Month;
            if (granuality == Granularity.Year) return (new DateTime(1, 1, 1) + (datum1.Timestamp - datum2.Timestamp)).Year - 1;

            return 0;
        }

        private Quality worstQuality(Quality quality1, Quality quality2)
        {
            if ((int)quality1 > (int)quality2) return quality1;
            return quality2;
        }

        private Path CreatePath(IEnumerable<string> components)
        {
            return Path.FromString(Path.JoinComponents(components));
        }

        private void CheckTimestamp(DateTime dt, Granularity granularity)
        {
            if(IsInvalid(dt, granularity))
                throw new DatetimeIsInvalidException(dt, granularity);
        }

        private bool IsInvalid(DateTime dt, Granularity granularity)
        {
            if ((int)granularity >= 0 && dt.Millisecond != 0) return true;
            if ((int)granularity >= 1 && dt.Second != 0) return true;
            if ((int)granularity >= 2 && dt.Minute != 0) return true;
            if ((int)granularity >= 3 && dt.Hour != 0) return true;
            if ((int)granularity == 4 && dt.DayOfWeek != DayOfWeek.Monday) return true;
            if ((int)granularity >= 5 && dt.Day != 1) return true;
            if ((int)granularity >= 6 && dt.Month != 1) return true;

            return false;
        }

        private void SetDefaultMissingValuePolicy(Signal signal)
        {
            var policy = typeof(NoneQualityMissingValuePolicy<>)
                .MakeGenericType(new Type[] { DataTypeUtils.GetNativeType(signal.DataType) });

            missingValuePolicyRepository.Set(signal, (MissingValuePolicyBase)Activator.CreateInstance(policy));
        }

        public void Delete<T>(int signalId)
        {
            var signal = signalsRepository.Get(signalId);
            if (signal == null) throw new ArgumentException("there is no signals with given id");

            SetMissingValuePolicy(signalId, null);
            signalsDataRepository.DeleteData<T>(signal);
            signalsRepository.Delete(signal);
        }
    }
}
