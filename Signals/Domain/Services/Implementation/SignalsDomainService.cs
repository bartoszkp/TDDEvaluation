using System.Collections.Generic;
using Domain.Repositories;

namespace Domain.Services.Implementation
{
    public class SignalsDomainService : ISignalsDomainService
    {
        private ISignalsRepository signalsRepository;

        public SignalsDomainService(ISignalsRepository signalsRepository)
        {
            this.signalsRepository = signalsRepository;
        }

        public Signal Get(Path path)
        {
            return this.signalsRepository.Get(path);
        }
    }
}
