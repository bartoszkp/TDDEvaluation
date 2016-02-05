using System;
using System.Collections.Generic;
using Domain.Repositories;
using Domain.Infrastructure;
using System.Linq;

namespace Domain.Services.Implementation
{
    public class SignalsDomainService : ISignalsDomainService
    {
        private readonly ISignalRepository signalRepository;

        public SignalsDomainService(ISignalRepository signalRepository)
        {
            this.signalRepository = signalRepository;
        }

        public Signal Get(Path path)
        {
            var result = this.signalRepository.Get(path);

            if (result == null)
            {
                throw new KeyNotFoundException();
            }

            return result;
        }

        public Signal Add(Signal signal)
        {
            return this.signalRepository.Add(signal);
        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncluded, DateTime toExcluded)
        {
            var readData = this.signalRepository.GetData<T>(signal, fromIncluded, toExcluded);
            return this.FillMissingData(signal, new TimeEnumerator(fromIncluded, toExcluded, signal.Granularity), readData);
        }

        public void SetData<T>(Signal signal, IEnumerable<Datum<T>> data)
        {
            foreach (var d in data)
            {
                d.Signal = signal;
                signal.Granularity.ValidateTimestamp(d.Timestamp);
            }

            this.signalRepository.SetData<T>(data);
        }

        private IEnumerable<Datum<T>> FillMissingData<T>(Signal signal, TimeEnumerator timeEnumerator, IEnumerable<Datum<T>> readData)
        {
            var leftJoin = from timestamp in timeEnumerator
                           join data in readData
                           on timestamp equals data.Timestamp into joined
                           from newData in joined.DefaultIfEmpty()
                           select new Datum<T>
                           {
                               Signal = signal,
                               Id = newData != null ? newData.Id : 0,
                               Timestamp = timestamp,
                               Quality = newData != null ? newData.Quality : Quality.None,
                               Value = newData != null ? newData.Value : default(T)
                           };
            return leftJoin.ToArray();
        }
    }
}
