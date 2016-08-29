using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Exceptions;
using Domain.Infrastructure;
using Domain.MissingValuePolicy;
using Domain.Repositories;
using Mapster;
using Domain.Services.Implementation.DataFillStrategy;
using Domain.DataFillStrategy;

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

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
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
                    this.missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<bool>());
                    break;

                case DataType.Integer:
                    this.missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<int>());
                    break;

                case DataType.Double:
                    this.missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<double>());
                    break;

                case DataType.Decimal:
                    this.missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<decimal>());
                    break;

                case DataType.String:
                    this.missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<string>());
                    break;
            }

            return result;
        }

        public Signal Get(Path pathDto)
        {
            return this.signalsRepository.Get(pathDto);
        }

        public void SetData<T>(IEnumerable<Datum<T>> data)
        {
            signalsDataRepository.SetData<T>(data);
        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var items = signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc).ToList();

            var mvp = GetMissingValuePolicy(signal.Id.GetValueOrDefault());

            var dataFillStrategy = DataFillStrategyProvider.GetStrategy(signal.Granularity, mvp);

            dataFillStrategy.FillMissingData(items, fromIncludedUtc, toExcludedUtc);


            var result = from d in items
                         orderby d.Timestamp
                         select d;

            return result;

        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicyBase domainPolicy)
        {
            var signal = this.GetById(signalId);
            if (signal == null)
                throw new NoSuchSignalException();

            this.missingValuePolicyRepository.Set(signal, domainPolicy);
        }

        public MissingValuePolicyBase GetMissingValuePolicy(int signalId)
        {
            var signal = this.GetById(signalId);
            if (signal == null)
                throw new NoSuchSignalException();

            var mvp = this.missingValuePolicyRepository.Get(signal);
            if (mvp != null)
                return TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType)
                as MissingValuePolicy.MissingValuePolicyBase;
            else
                return null;
        }

        public PathEntry GetPathEntry(Path path)
        {
            var result = signalsRepository.GetAllWithPathPrefix(path);
            int pathLength = path.Components.Count();

            List<Signal> signals = new List<Signal>();
            foreach (var s in result)
                if (pathLength + 1 == s.Path.Components.Count())
                    signals.Add(s);

            List<Path> paths = new List<Path>();
            foreach (var s in result)
                if (pathLength + 2 <= s.Path.Components.Count())
                {
                    List<string> newPath = new List<string>();
                    int i = 0;
                    foreach (var p in s.Path.Components)
                    {
                        newPath.Add(p);
                        if (++i >= pathLength + 1)
                            break;
                    }
                    var pathString = Path.FromString(string.Join("/", newPath));
                    if (pathIsAdded(paths, pathString))
                        paths.Add(pathString);
                }

            return new PathEntry(signals, paths);
        }

        private bool pathIsAdded(List<Path> paths, Path path)
        {
            foreach (var p in paths)
                if (p.ToString() == path.ToString())
                    return false;
            return true;
        }

        public void Delete(int signalId)
        {
            var signal = GetById(signalId);
            SetMissingValuePolicy(signalId, null);
            signalsRepository.Delete(signal);
        }
    }


}
