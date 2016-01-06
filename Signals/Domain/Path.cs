using System.Collections.Generic;

namespace Signals.Domain
{
    public class Path
    {
        public IEnumerable<string> Components { get; set; }

        public override string ToString()
        {
            return string.Join("/", Components);
        }
    }
}
