using System.Collections.Generic;

namespace Domain
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

        public override bool Equals(object obj)
        {
            Path pathObj = obj as Path;
            return (pathObj != null) && (ToString() == pathObj.ToString());
        }

        public override int GetHashCode()
        {
            return this.Components.GetHashCode();
        }
    }
}
