using System.Collections.Generic;

namespace Dto
{
    public class PathEntry
    {
        public IEnumerable<Signal> Signals { get; set; }

        public IEnumerable<Path> SubPaths { get; set; }
    }
}
