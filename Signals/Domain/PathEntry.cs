using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    [Infrastructure.NHibernateIgnore]
    public class PathEntry
    {
        public IEnumerable<Signal> Signals { get; private set; }

        public IEnumerable<Path> SubPaths { get; private set; }

        public PathEntry()
        {
            Signals = Enumerable.Empty<Signal>();
            SubPaths = Enumerable.Empty<Path>();
        }

        public PathEntry(IEnumerable<Signal> signals, IEnumerable<Path> subPaths)
        {
            Signals = signals;
            SubPaths = subPaths;
        }
    }
}
