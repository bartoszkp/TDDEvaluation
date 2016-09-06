using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Domain;
using Domain.Infrastructure;
using Domain.MissingValuePolicy;
using Mapster;
using NHibernate.Criterion;

namespace DataAccess.Repositories
{
    [UnityRegister]
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

            if (missingValuePolicy == null)
            {
                return;
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
            var signalPropertyName = ReflectionUtils.GetMemberInfo<MissingValuePolicyBase, Signal>(mvpb => mvpb.Signal)
                .Name;

            foreach (var type in genericConcretePolicyTypePairs
                .Select(gcptp => gcptp.Item2)
                .Where(gcpt => gcpt.BaseType.GetGenericArguments().Single().Equals(signal.DataType.GetNativeType())))
            {
                var tryGet = Session.CreateCriteria(type)
                    .Add(Restrictions.Eq(signalPropertyName, signal))
                    .SetMaxResults(1)
                    .List()
                    .Cast<MissingValuePolicyBase>()
                    .SingleOrDefault();

                if (tryGet != null)
                {
                    return tryGet;
                }
            }

            return null;
        }

        private List<Tuple<Type, Type>> genericConcretePolicyTypePairs = new List<Tuple<Type, Type>>();
    }
}
