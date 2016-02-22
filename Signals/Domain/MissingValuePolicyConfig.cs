using System.Collections.Generic;

namespace Domain
{
    public class MissingValuePolicyConfig
    {
        public virtual int Id { get; set; }

        public virtual Signal Signal { get; set; }

        public virtual MissingValuePolicy Policy { get; set; }

        public virtual IDictionary<string, object> Params { get; protected set; }

        public MissingValuePolicyConfig()
        {
            Params = new Dictionary<string, object>();
        }
    }
}