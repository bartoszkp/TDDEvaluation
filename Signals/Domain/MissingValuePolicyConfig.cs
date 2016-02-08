using System.Collections.Generic;

namespace Domain
{
    public class MissingValuePolicyConfig
    {
        public virtual int Id { get; set; }

        public virtual Signal Signal { get; set; }

        public virtual MissingValuePolicy Policy { get; set; }
  // TODO      public virtual IDictionary<string, object> Params { get; protected set; }

        public MissingValuePolicyConfig()
        {
         // TODO   Params = new Dictionary<string, object>();
        }
    }
}