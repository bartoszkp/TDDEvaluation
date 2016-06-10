using Domain.MissingValuePolicy;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Mapster;

namespace DataAccess.Repositories
{
    public class MissingValuePolicyRepository : RepositoryBase, Domain.Repositories.IMissingValuePolicyRepository
    {
        public MissingValuePolicyRepository(ISessionProvider sessionProvider)
            : base(sessionProvider)
        {
            var specificValuePolicyTypes = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.BaseType != null)
                .Where(t => t.BaseType.IsGenericType)
                .Where(t => t.BaseType.GetGenericTypeDefinition().Equals(typeof(SpecificValueMissingValuePolicy<>)))
                .ToArray();

            foreach (var specificValuePolicyType in specificValuePolicyTypes)
            {
                genericConcreteSpecificMissingValuePolicyTypePairs.Add(Tuple.Create(specificValuePolicyType.BaseType, specificValuePolicyType));
            }
        }

        public void Add(MissingValuePolicy missingValuePolicy)
        {
            if (missingValuePolicy.GetType().IsGenericType && missingValuePolicy.GetType().GetGenericTypeDefinition().Equals(typeof(SpecificValueMissingValuePolicy<>)))
            {
                var mappedPolicy = TypeAdapter.Adapt(
                    missingValuePolicy,
                    missingValuePolicy.GetType(),
                    GetConcreteSpecificValuePolicyType(missingValuePolicy),
                    new TypeAdapterConfig());

                Session.Save(mappedPolicy);
            }
            else
            {
                Session.Save(missingValuePolicy);
            }
        }

        public void Delete(MissingValuePolicy missingValuePolicy)
        {
            Session.Delete(missingValuePolicy);
        }

        public Domain.MissingValuePolicy.MissingValuePolicy Get(int signalId)
        {
            return Session
                .QueryOver<Domain.MissingValuePolicy.MissingValuePolicy>()
                .Where(mvp => mvp.Signal.Id == signalId)
                .SingleOrDefault();
        }

        private Type GetConcreteSpecificValuePolicyType(MissingValuePolicy specificMissingValuePolicy)
        {
            var genericArgument = specificMissingValuePolicy.GetType().GetGenericArguments().Single();

            var concreteSpecificValuePolicyType = genericConcreteSpecificMissingValuePolicyTypePairs
                .Single(pair => pair.Item1.GenericTypeArguments.Single().Equals(genericArgument))
                .Item2;

            return concreteSpecificValuePolicyType;
        }

        private List<Tuple<Type, Type>> genericConcreteSpecificMissingValuePolicyTypePairs = new List<Tuple<Type, Type>>();
    }
}

