using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace WebService.Tests.SignalsWebServiceTests.Infrastructure
{
    public static class Utils
    {
        public static readonly DateTime validTimestamp = new DateTime(2000, 1, 1, 0, 0, 0);

        public static Dto.Signal SignalWith(
            int? id = null,
            Dto.DataType dataType = Dto.DataType.Double,
            Dto.Granularity granularity = Dto.Granularity.Month,
            Dto.Path path = null)
        {
            return new Dto.Signal()
            {
                Id = id,
                DataType = dataType,
                Granularity = granularity,
                Path = path
            };
        }

        public static Domain.Signal SignalWith(
            int? id = null,
            Domain.DataType dataType = Domain.DataType.Double,
            Domain.Granularity granularity = Domain.Granularity.Month,
            Domain.Path path = null)
        {
            return new Domain.Signal()
            {
                Id = id,
                DataType = dataType,
                Granularity = granularity,
                Path = path
            };
        }

        public static bool CompareSignal(Dto.Signal a, Dto.Signal b)
        {
            return a.DataType == b.DataType &&
                a.Granularity == b.Granularity &&
                a.Id == b.Id &&
                PathToString(a.Path) == PathToString(b.Path); 
        }

        public static bool CompareDatum(Dto.Datum[] a, Dto.Datum[] b)
        {
            if(a.Length != b.Length) return false;

            for (int i = 0; i < a.Length; i++)
                if (CompareDatum(a[i], b[i]) == false) return false;

            return true;
        }

        public static bool CompareDatum(Dto.Datum a, Dto.Datum b)
        {
            return a.Quality == b.Quality &&
                a.Timestamp == b.Timestamp &&
                a.Value.ToString() == b.Value.ToString();
        }

        public static bool ComparePath(Domain.Path a, Domain.Path b)
        {
            return PathToString(a) == PathToString(b);
        }

        private static string PathToString(Dto.Path path)
        {
            return string.Join("/", path.Components.ToArray());
        }

        private static string PathToString(Domain.Path path)
        {
            return string.Join("/", path.Components.ToArray());
        }
    }
}
