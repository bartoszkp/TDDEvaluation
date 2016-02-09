using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Domain;
using Mapster;
using NHibernate.Criterion;

namespace DataAccess.Repositories
{
    public class SignalsRepository : RepositoryBase, Domain.Repositories.ISignalsRepository
    {
        public SignalsRepository(ISessionProvider sessionProvider)
            : base(sessionProvider)
        {
            var datumTypes = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.BaseType != null)
                .Where(t => t.BaseType.IsGenericType)
                .Where(t => t.BaseType.GetGenericTypeDefinition().Equals(typeof(Datum<>)))
                .ToArray();

            foreach (var datumType in datumTypes)
            {
                genericConcreteDatumTypePairs.Add(Tuple.Create(datumType.BaseType, datumType));
            }
        }

        public Signal Add(Signal signal)
        {
            Session.SaveOrUpdate(signal);

            return signal;
        }

        public Signal Get(Path path)
        {
            return Session
                .QueryOver<Signal>()
                .Where(s => s.Path == path)
                .SingleOrDefault();
        }

        public void Remove(Path path)
        {
            var signal = Get(path);

            Session.Delete(signal);
        }

        public void SetData<T>(IEnumerable<Datum<T>> data)
        {
            var concreteDatumType = GetConcreteDatumType<T>();

            foreach (var datum in data)
            {
                var mappedDatum = TypeAdapter.Adapt(datum, datum.GetType(), concreteDatumType);

                Session.SaveOrUpdate(mappedDatum);
            }
        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncluded, DateTime toExcluded)
        {
            var concreteDatumType = GetConcreteDatumType<T>();

            return Session
                .CreateCriteria(concreteDatumType)
                .Add(Restrictions.Eq("Signal", signal))
                .Add(Restrictions.Between("Timestamp", fromIncluded, toExcluded))
                .List()
                .Cast<Datum<T>>();
        }

        private Type GetConcreteDatumType<T>()
        {
            var concreteDatumType = genericConcreteDatumTypePairs
                .Single(pair => pair.Item1.GenericTypeArguments.Single().Equals(typeof(T)))
                .Item2;

            return concreteDatumType;
        }

        private List<Tuple<Type, Type>> genericConcreteDatumTypePairs = new List<Tuple<Type, Type>>();
    }
}
