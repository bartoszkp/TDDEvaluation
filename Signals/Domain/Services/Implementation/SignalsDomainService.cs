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

            dataFillStrategy.FillMissingData(signal,items, fromIncludedUtc, toExcludedUtc,this.signalsDataRepository);


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

            if (!IsShadowSignalMatch(signal, domainPolicy))
                throw new InvalidOperationException("Shadow Signal type or granaulity dosen't match");

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
            if (signal == null)
                throw new IdNotNullException();
            SetMissingValuePolicy(signalId, null);
            switch(signal.DataType)
            {
                case DataType.Boolean:
                    signalsDataRepository.DeleteData<bool>(signal);
                    break;
                case DataType.Decimal:
                    signalsDataRepository.DeleteData<decimal>(signal);
                    break;
                case DataType.Double:
                    signalsDataRepository.DeleteData<double>(signal);
                    break;
                case DataType.Integer:
                    signalsDataRepository.DeleteData<int>(signal);
                    break;
                case DataType.String:
                    signalsDataRepository.DeleteData<string>(signal);
                    break;
            }
            signalsRepository.Delete(signal);
        }

        private bool IsShadowSignalMatch(Signal signal, MissingValuePolicyBase domainPolicy)
        {
            Signal signalShadow;

            if (domainPolicy.GetType() == typeof(MissingValuePolicy.ShadowMissingValuePolicy<bool>))
                signalShadow = ((ShadowMissingValuePolicy<bool>)domainPolicy).ShadowSignal;
            else if (domainPolicy.GetType() == typeof(MissingValuePolicy.ShadowMissingValuePolicy<int>))
                signalShadow = ((ShadowMissingValuePolicy<int>)domainPolicy).ShadowSignal;
            else if (domainPolicy.GetType() == typeof(MissingValuePolicy.ShadowMissingValuePolicy<double>))
                signalShadow = ((ShadowMissingValuePolicy<double>)domainPolicy).ShadowSignal;
            else if (domainPolicy.GetType() == typeof(MissingValuePolicy.ShadowMissingValuePolicy<decimal>))
                signalShadow = ((ShadowMissingValuePolicy<decimal>)domainPolicy).ShadowSignal;
            else if (domainPolicy.GetType() == typeof(MissingValuePolicy.ShadowMissingValuePolicy<string>))
                signalShadow = ((ShadowMissingValuePolicy<string>)domainPolicy).ShadowSignal;
            else
                signalShadow = null;

            if (signal.DataType != signalShadow.DataType || signal.Granularity != signalShadow.Granularity)
                return false;

            return true;
        }

    }


}
