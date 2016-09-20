using System.Collections.Generic;
using System.Linq;

namespace SignalsIntegrationTests.Infrastructure
{
    public static class TimeoutRegistry
    {
        private static HashSet<string> TimeoutCategories = new HashSet<string>();

        public static void RegisterTimeout(IEnumerable<string> categories)
        {
            lock (TimeoutCategories)
            {
                foreach (var c in categories)
                {
                    TimeoutCategories.Add(c);
                }
            }
        }

        public static bool ContainsAll(IEnumerable<string> categories)
        {
            return categories.All(c => TimeoutCategories.Contains(c));
        }
    }
}
