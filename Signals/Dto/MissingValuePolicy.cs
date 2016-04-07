using System.Runtime.Serialization;

namespace Dto
{
    [DataContract]
    [KnownType(typeof(NoneQualityMissingValuePolicy))]
    public abstract class MissingValuePolicy
    {
    }
}