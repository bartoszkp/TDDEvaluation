using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DataAccess.GenericInstantiations;
using Domain;
using FastMapper;
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
                .Where(t => t.BaseType.GetGenericTypeDefinition().Equals(typeof(Domain.Datum<>)))
                .ToArray();

            foreach (var datumType in datumTypes)
            {
                genericTypeMappinges.Add(datumType.BaseType, datumType);
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
            foreach (var datum in data)
            {
                var mappingType = genericTypeMappinges[datum.GetType()];

                var mappedDatum = TypeAdapter.Adapt(datum, datum.GetType(), mappingType);

                Session.SaveOrUpdate(mappedDatum);
            }
        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncluded, DateTime toExcluded)
        {
            var dictionary = new Dictionary<Type, Type>()
            {
              { typeof(bool), typeof(DatumBoolean) },
              { typeof(int), typeof(DatumInteger) },
              { typeof(double), typeof(DatumDouble) },
              { typeof(decimal), typeof(DatumDecimal) }
            };

            var type = dictionary[typeof(T)];

            return Session
                .CreateCriteria(type)
                .Add(Expression.Eq("Signal", signal))
                .Add(Expression.Between("Timestamp", fromIncluded, toExcluded))
                .List()
                .Cast<Datum<T>>();
        }

        private Dictionary<Type, Type> genericTypeMappinges = new Dictionary<Type, Type>(); 
    }
}
