using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Domain;
using Domain.Infrastructure;
using Mapster;
using NHibernate.Criterion;

namespace DataAccess.Repositories
{
    public class SignalsDataRepository : RepositoryBase, Domain.Repositories.ISignalsDataRepository
    {
        public SignalsDataRepository(ISessionProvider sessionProvider)
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

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var concreteDatumType = GetConcreteDatumType<T>();

            var signalPropertyName = GetDatumPropertyName<T>(d => d.Signal);
            var timestampPropertyName = GetDatumPropertyName<T>(d => d.Timestamp);

            return Session
                .CreateCriteria(concreteDatumType)
                .Add(Restrictions.Eq(signalPropertyName, signal))
                .Add(Restrictions.Ge(timestampPropertyName, fromIncludedUtc))
                .Add(Restrictions.Lt(timestampPropertyName, toExcludedUtc))
                .List()
                .Cast<Datum<T>>();
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

        private Type GetConcreteDatumType<T>()
        {
            var concreteDatumType = genericConcreteDatumTypePairs
                .Single(pair => pair.Item1.GenericTypeArguments.Single().Equals(typeof(T)))
                .Item2;

            return concreteDatumType;
        }

        private string GetDatumPropertyName<T>(Expression<Func<Datum<T>, object>> expression)
        {
            return ReflectionUtils.GetMemberInfo(expression).Name;
        }

        private List<Tuple<Type, Type>> genericConcreteDatumTypePairs = new List<Tuple<Type, Type>>();
    }
}
