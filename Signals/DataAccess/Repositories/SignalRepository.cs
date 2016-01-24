using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.GenericInstantiations;
using Domain;
using NHibernate.Criterion;

namespace DataAccess.Repositories
{
    public class SignalRepository : RepositoryBase, Domain.Repositories.ISignalRepository
    {
        public SignalRepository(ISessionProvider sessionProvider)
            : base(sessionProvider)
        {
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
                Session.SaveOrUpdate(datum);
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
    }
}
