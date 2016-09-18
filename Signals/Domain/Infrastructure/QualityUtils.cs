using System;
using System.Linq;

namespace Domain.Infrastructure
{
    public static class QualityUtils
    {
        public static Quality GetMinQuality(Quality a, Quality b)
        {
            if (a == Quality.None || b == Quality.None)
                return Quality.None;

            return Enum
                .GetValues(typeof(Quality))
                .Cast<Quality>()
                .Last(q => q == a || q == b);
        }
    }
}
