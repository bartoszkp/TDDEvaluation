using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Exceptions;
using Domain.Infrastructure;
using Domain.MissingValuePolicy;
using Domain.Repositories;
using Mapster;
using Domain;
using System.Collections;

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

            var signal = this.signalsRepository.Add(newSignal);

            if (missingValuePolicyRepository == null)
            {
                return signal;
            }
            
            string typeName = signal.DataType.GetNativeType().Name;

            switch (typeName)
            {
                case "Int32":
                    GenericSetDataCall<int>(signal);
                    break;
                case "Double":
                    GenericSetDataCall<double>(signal);
                    break;
                case "Decimal":
                    GenericSetDataCall<decimal>(signal);
                    break;
                case "Boolean":
                    GenericSetDataCall<bool>(signal);
                    break;
                case "String":
                    GenericSetDataCall<string>(signal);
                    break;
            }

            return signal;
        }

        private void GenericSetDataCall<T>(Signal signal)
        {
            this.missingValuePolicyRepository.Set(signal, new NoneQualityMissingValuePolicy<T>());
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public Signal Get(Path path)
        {
            var result = signalsRepository.Get(path);
            if (result == null)
                return null;
            return result;
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicyBase domainPolicyBase)
        {
            var gettingSignalFromRepository = signalsRepository.Get(signalId);
            if (gettingSignalFromRepository == null)
                throw new SignalIsNotException();
            missingValuePolicyRepository.Set(gettingSignalFromRepository, domainPolicyBase);
        }

        public MissingValuePolicyBase GetMissingValuePolicy(Signal signalDomain)
        {
            var result = this.missingValuePolicyRepository.Get(signalDomain);
            if (result == null)
                throw new IdNotNullException();
            else
                return TypeAdapter.Adapt(result, result.GetType(), result.GetType().BaseType) as MissingValuePolicy.MissingValuePolicyBase;
        }

        public void SetData<T>(Signal signal, IEnumerable<Datum<T>> data)
        {
            if (!isTimeStampValid(signal, data))
            {
                throw new Domain.Exceptions.InvalidTimeStampException();
            }

            if(data == null)
            {
                this.signalsDataRepository.SetData<T>(data);
                return;
            }

            foreach(var d in data)
            {
                d.Signal = signal;
            }

            this.signalsDataRepository.SetData<T>(data);
        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUTC, DateTime toExcludedUTC)
        {
            if (fromIncludedUTC > toExcludedUTC)
                return new List<Datum<T>>();

            List<Datum<T>> dataToCheck = new List<Datum<T>>();
            dataToCheck.Add(new Datum<T>() { Timestamp = fromIncludedUTC });
            dataToCheck.Add(new Datum<T>() { Timestamp = toExcludedUTC });

            if (!isTimeStampValid(signal, dataToCheck))
            {
                throw new Domain.Exceptions.InvalidTimeStampException();
            }

            MissingValuePolicy<T> missingValuePolicy;

            var data = this.signalsDataRepository
                .GetData<T>(signal, fromIncludedUTC, toExcludedUTC)?.ToArray();

            if (data == null)
                return null;

            if (data.Count() == 0 || data.First().Timestamp != fromIncludedUTC)
            {
                List<Datum<T>> dataToFill = data.ToList();
                dataToFill.Add(new Datum<T>()
                {
                    Id = 0,
                    Quality = Quality.None,
                    Signal = signal,
                    Timestamp = fromIncludedUTC,
                    Value = default(T),
                });

                data = dataToFill.OrderBy(s => s.Timestamp).ToArray();
            }

            if (fromIncludedUTC == toExcludedUTC)
                return data;

            var timestampBegin = fromIncludedUTC;
            var timestampEnd = toExcludedUTC;
            var dateTimeComparator = DateTime.Compare(timestampBegin, timestampEnd);

            if(dateTimeComparator > 0)
            {
                for (int i = 0; i < data.Count(); i++)
                {
                    data.ToList().RemoveAt(i);
                }

                missingValuePolicy = GetMissingValuePolicy(signal)
                as MissingValuePolicy.MissingValuePolicy<T>;

                return missingValuePolicy.FillData(signal, data, fromIncludedUTC, toExcludedUTC).ToArray();
            }
            else
                missingValuePolicy = GetMissingValuePolicy(signal)
                as MissingValuePolicy.MissingValuePolicy<T>;

                return missingValuePolicy.FillData(signal, data, fromIncludedUTC, toExcludedUTC).ToArray();
        }

        public PathEntry GetPathEntry(Path path)
        {
            var result = signalsRepository.GetAllWithPathPrefix(path);

            List<Signal> signals = new List<Signal>();
            List<Path> paths = new List<Path>();

            foreach (var signal in result)
            {
                if(signal.Path.ToString().LastIndexOf('/') == path.ToString().Length)
                    signals.Add(signal);

                var pathToAdd = signal.Path.GetPrefix(path.Length + 1);
                if (pathToAdd.ToString() != signal.Path.ToString() && !paths.Contains(pathToAdd))
                    paths.Add(pathToAdd);
            }

            return new PathEntry(signals, paths);
        }

        private bool isTimeStampValid<T>(Signal signal, IEnumerable<Datum<T>> data)
        {
            foreach (var datum in data)
            {
                switch (signal.Granularity)
                {
                    case Granularity.Second:
                        if (!IsZero(datum.Timestamp.Millisecond))
                            return false;
                        break;

                    case Granularity.Minute:
                        if (!IsZero(datum.Timestamp.Millisecond, datum.Timestamp.Second))
                            return false;
                        break;

                    case Granularity.Hour:
                        if (!IsZero(datum.Timestamp.Millisecond, datum.Timestamp.Second, datum.Timestamp.Minute))
                            return false;
                        break;

                    case Granularity.Day:
                        if (!IsZero(datum.Timestamp.Millisecond, datum.Timestamp.Second, datum.Timestamp.Minute, datum.Timestamp.Hour))
                            return false;
                        break;

                    case Granularity.Week:
                        if (!IsZero(datum.Timestamp.Millisecond, datum.Timestamp.Second, datum.Timestamp.Minute, datum.Timestamp.Hour, datum.Timestamp.DayOfWeek))
                            return false;
                        break;

                    case Granularity.Month:
                        if (!IsZero(datum.Timestamp.Millisecond, datum.Timestamp.Second, datum.Timestamp.Minute, datum.Timestamp.Hour, DayOfWeek.Monday, datum.Timestamp.Day))
                            return false;
                        break;

                    case Granularity.Year:
                        if (!IsZero(datum.Timestamp.Millisecond, datum.Timestamp.Second, datum.Timestamp.Minute, datum.Timestamp.Hour, DayOfWeek.Monday, datum.Timestamp.Day, datum.Timestamp.Month))
                            return false;
                        break;

                    default:
                        break;
                }
            }

            return true;
        }

        private bool IsZero(int millisecond, int second = 0, int minute = 0, int hour = 0, DayOfWeek dayOfWeek = DayOfWeek.Monday, int day = 1, int month = 1)
        {
            if (millisecond == 0 && second == 0 && minute == 0 && hour == 0 && dayOfWeek == DayOfWeek.Monday && day == 1 && month == 1)
                return true;
            return false;
        }
    }
}
