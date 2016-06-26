using Domain.MissingValuePolicy;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System;
using Mapster;
using Domain.Infrastructure;

namespace DataAccess.Repositories
{
    public class MissingValuePolicyRepository : RepositoryBase, Domain.Repositories.IMissingValuePolicyRepository
    {
        public MissingValuePolicyRepository(ISessionProvider sessionProvider)
            : base(sessionProvider)
        {
            var policyTypes = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.BaseType != null)
                .Where(t => t.BaseType.BaseType != null)
                .Where(t => t.BaseType.BaseType.IsGenericType)
                .Where(t => t.BaseType.BaseType.GetGenericTypeDefinition().Equals(typeof(MissingValuePolicy<>)))
                .ToArray();

            foreach (var policyType in policyTypes)
            {
                genericConcretePolicyTypePairs.Add(Tuple.Create(policyType.BaseType, policyType));
            }
        }

        public void Set(Domain.Signal signal, MissingValuePolicyBase missingValuePolicy)
        {
            var existing = Get(signal);

            if (existing != null)
            {
                Session.Delete(existing);
            }

            var concretePolicyType = GetConcretePolicyType(missingValuePolicy);

            var concretePolicy = TypeAdapter.Adapt(missingValuePolicy, missingValuePolicy.GetType(), concretePolicyType)
                as MissingValuePolicyBase;

            concretePolicy.Signal = signal;

            Session.SaveOrUpdate(concretePolicy);
        }

        private Type GetConcretePolicyType(MissingValuePolicyBase missingValuePolicy)
        {
            var concretePolicyType = genericConcretePolicyTypePairs
                .Single(pair => pair.Item1.Equals(missingValuePolicy.GetType()))
                .Item2;

            return concretePolicyType;
        }

        public MissingValuePolicyBase Get(Domain.Signal signal)
        {
            return Session.QueryOver<MissingValuePolicyBase>()
                .Where(mvp => mvp.Signal == signal)
                .SingleOrDefault();
        }

        private List<Tuple<Type, Type>> genericConcretePolicyTypePairs = new List<Tuple<Type, Type>>();
    }
}
