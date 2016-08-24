using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace WebService.Tests
{
    public static class Utils
    {
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

        private static string PathToString(Dto.Path path)
        {
            return string.Join("/", path.Components.ToArray());
        }
    }
}
