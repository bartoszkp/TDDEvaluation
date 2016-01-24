using System;
using System.Collections.Generic;
using Domain.Repositories;

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
            return this.signalRepository.GetData<T>(signal, fromIncluded, toExcluded);
        }

        public void SetData<T>(Signal signal, IEnumerable<Datum<T>> data)
        {
            foreach (var d in data)
            {
                d.Signal = signal;
            }

            this.signalRepository.SetData<T>(data);
        }
    }
}
