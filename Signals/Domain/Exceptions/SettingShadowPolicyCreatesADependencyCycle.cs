using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class SettingShadowPolicyCreatesADependencyCycle : Exception
    {
        public SettingShadowPolicyCreatesADependencyCycle()
            : base("Setting Shadow Policy creates a dependency cycle")
        {
        }
    }
}
