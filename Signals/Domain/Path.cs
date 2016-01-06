using System.Collections.Generic;

namespace Signals.Domain
{
    public class Path
    {
        public IEnumerable<string> Components { get; set; }

        public static Path FromString(string str)
        {
            return new Path()
            {
                Components = str.Split('/')
            };
        }

        public override string ToString()
        {
            return string.Join("/", Components);
        }
    }
}
