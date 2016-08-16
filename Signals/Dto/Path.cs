using System;
using System.Collections.Generic;
using System.Linq;

namespace Dto
{
    public class Path 
    {
        public IEnumerable<string> Components { get; set; }
    }
    public class PathComparer : IEqualityComparer<Path>
    {
        public bool Equals(Path x, Path y)
        {
            if (Object.ReferenceEquals(x, y)) return true;
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null)) return false;

            return (x.Components.SequenceEqual(y.Components));
        }

        public int GetHashCode(Path obj)
        {
            return obj.Components == null ? 0 : obj.Components.First().GetHashCode();
        }
    }
}
