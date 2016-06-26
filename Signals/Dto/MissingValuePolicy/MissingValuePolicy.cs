using Dto.Conversions;
using System.Runtime.Serialization;

namespace Dto.MissingValuePolicy
{
    [DataContract]
    [KnownType(typeof(NoneQualityMissingValuePolicy))]
    [KnownType(typeof(SpecificValueMissingValuePolicy))]
    public abstract class MissingValuePolicy
    {
        [DataMember]
        public int? Id { get; set; }

        [DataMember]
        public Signal Signal { get; set; }

        [DataMember]
        [MapFromGenericDataType]
        public DataType DataType { get; set; }
    }
}