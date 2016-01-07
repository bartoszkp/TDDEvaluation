using System.Collections.Generic;

namespace Domain
{
    public class Path
    {
        public const char Delimiter = '/';

        public static IEnumerable<string> ParseComponents(string str)
        {
            return str.Split(Delimiter);
        }

        public static string JoinComponents(IEnumerable<string> components)
        {
            return string.Join(Delimiter.ToString(), components);
        }

        public static Path FromString(string str)
        {
            return new Path()
            {
                Components = ParseComponents(str)
            };
        }

        public virtual IEnumerable<string> Components { get; set; }

        public override string ToString()
        {
            return JoinComponents(this.Components);
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
