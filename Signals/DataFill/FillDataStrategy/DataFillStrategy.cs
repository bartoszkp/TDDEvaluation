using Domain.MissingValuePolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.FillDataStrategy
{
    public abstract class DataFillStrategy
    {
        protected MissingValuePolicyBase missingValuePolicy;

        public abstract void FillMissingData<T>(List<Domain.Datum<T>> datum, DateTime after, DateTime before);
    }
}
