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
    [UnityRegister]
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

            var query = Session
                .CreateCriteria(concreteDatumType)
                .Add(Restrictions.Eq(signalPropertyName, signal))
                .Add(Restrictions.Ge(timestampPropertyName, fromIncludedUtc));

            if (toExcludedUtc > fromIncludedUtc)
            {
                query = query.Add(Restrictions.Lt(timestampPropertyName, toExcludedUtc));
            }
            else
            {
                query = query.Add(Restrictions.Le(timestampPropertyName, toExcludedUtc));
            }

            return query
                .List()
                .Cast<Datum<T>>();
        }

        public void SetData<T>(IEnumerable<Datum<T>> data)
        {
            if (!data.Any())
            {
                return;
            }

            var concreteDatumType = GetConcreteDatumType<T>();

            var firstTimestamp = data.Min(d => d.Timestamp);
            var lastTimestamp = data.Max(d => d.Timestamp);

            var existingData = data.Any()
                       ? GetData<T>(data.First().Signal, firstTimestamp, lastTimestamp.AddSeconds(1))
                         .ToDictionary(d => d.Timestamp)
                       : new Dictionary<DateTime, Datum<T>>();

            foreach (var datum in data)
            {
                Datum<T> existingDatum = null;
                if (existingData.TryGetValue(datum.Timestamp, out existingDatum))
                {
                    existingDatum.Value = datum.Value;
                    existingDatum.Quality = datum.Quality;
                }
                else
                {
                    var mappedDatum = TypeAdapter.Adapt(datum, datum.GetType(), concreteDatumType);
                    Session.SaveOrUpdate(mappedDatum);
                }
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

        public IEnumerable<Datum<T>> GetDataOlderThan<T>(Signal signal, DateTime excludedUtc, int maxSampleCount)
        {
            if (maxSampleCount < 1)
                return Enumerable.Empty<Datum<T>>();

            var concreteDatumType = GetConcreteDatumType<T>();

            var signalPropertyName = GetDatumPropertyName<T>(d => d.Signal);
            var timestampPropertyName = GetDatumPropertyName<T>(d => d.Timestamp);

            return Session
                .CreateCriteria(concreteDatumType)
                .Add(Restrictions.Eq(signalPropertyName, signal))
                .Add(Restrictions.Lt(timestampPropertyName, excludedUtc))
                .AddOrder(Order.Desc(timestampPropertyName))
                .SetMaxResults(maxSampleCount)
                .List()
                .Cast<Datum<T>>();
        }

        public IEnumerable<Datum<T>> GetDataNewerThan<T>(Signal signal, DateTime includedUtc, int maxSampleCount)
        {
            if (maxSampleCount < 1)
                return Enumerable.Empty<Datum<T>>();

            var concreteDatumType = GetConcreteDatumType<T>();

            var signalPropertyName = GetDatumPropertyName<T>(d => d.Signal);
            var timestampPropertyName = GetDatumPropertyName<T>(d => d.Timestamp);

            return Session
                .CreateCriteria(concreteDatumType)
                .Add(Restrictions.Eq(signalPropertyName, signal))
                .Add(Restrictions.Ge(timestampPropertyName, includedUtc))
                .AddOrder(Order.Asc(timestampPropertyName))
                .SetMaxResults(maxSampleCount)
                .List()
                .Cast<Datum<T>>();
        }

        public void DeleteData<T>(Signal signal)
        {
            var concreteDatumType = GetConcreteDatumType<T>();

            var signalPropertyName = GetDatumPropertyName<T>(d => d.Signal);

            foreach (var toDelete in Session
                .CreateCriteria(concreteDatumType)
                .Add(Restrictions.Eq(signalPropertyName, signal))
                .List())
            {
                Session.Delete(toDelete);
            }
        }

        private List<Tuple<Type, Type>> genericConcreteDatumTypePairs = new List<Tuple<Type, Type>>();
    }
}
