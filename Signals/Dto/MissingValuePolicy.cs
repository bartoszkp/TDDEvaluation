using System.Runtime.Serialization;

namespace Dto
{
    [DataContract]
    [KnownType(typeof(NoneQualityMissingValuePolicy))]
    public abstract class MissingValuePolicy
    {
        [DataMember]
        public int? Id { get; set; }

        [DataMember]
        public Signal Signal { get; set; }
    }
}