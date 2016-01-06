using System.Collections.Generic;

namespace Signals.Domain
{
    public class Path
    {
        public IEnumerable<string> Components { get; set; }

        public static Path FromString(string str)
        {
            var path = new Path();
            path.Components = str.Split('/');
            return path;
        }

        public override string ToString()
        {
            return string.Join("/", Components);
        }
    }
}
