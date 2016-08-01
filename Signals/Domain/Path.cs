using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    [Infrastructure.Component]
    public class Path
    {
        public const char Delimiter = '/';

        public static readonly Path Root = Path.FromString(string.Empty);

        public static IEnumerable<string> ParseComponents(string str)
        {
            return str.Split(new[] { Delimiter }, StringSplitOptions.RemoveEmptyEntries);
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

        public static Path operator+(Path p1, Path p2)
        {
            return new Path()
            {
                Components = p1.Components.Concat(p2.Components).ToArray()
            };
        }

        public static Path operator+(Path p1, string postfix)
        {
            return new Path()
            {
                Components = p1.Components.Concat(ParseComponents(postfix)).ToArray()
            };
        }

        private string[] components;
        public virtual IEnumerable<string> Components
        {
            get
            {
                return components;
            }
            protected set
            {
                components = value.ToArray();
            }
        }

        [Infrastructure.NHibernateIgnore]
        public virtual int Length { get { return this.components.Count(); } }

        public virtual Path GetPrefix(int numberOfComponents)
        {
            return new Path()
            {
                Components = this.components.Take(numberOfComponents)
            };
        }

        public override string ToString()
        {
            return JoinComponents(this.components);
        }

        public override bool Equals(object obj)
        {
            Path pathObj = obj as Path;
            return (pathObj != null) && (ToString() == pathObj.ToString());
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }
}
