using System.Collections.Generic;

namespace Dto
{
    public class MissingValuePolicyConfig
    {
        public MissingValuePolicy Policy { get; set; }

        public IDictionary<string, object> Params { get; set; }
    }
}