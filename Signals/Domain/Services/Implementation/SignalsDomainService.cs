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
            var result = this.signalsRepository.Get(path);

            if (result == null)
            {
                throw new KeyNotFoundException();
            }

            return result;
        }
    }
}
