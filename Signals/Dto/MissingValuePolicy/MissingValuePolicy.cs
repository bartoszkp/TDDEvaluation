using System.Runtime.Serialization;
using Dto.Conversions;

namespace Dto.MissingValuePolicy
{
    [DataContract]
    [KnownType(typeof(NoneQualityMissingValuePolicy))]
    [KnownType(typeof(SpecificValueMissingValuePolicy))]
    [KnownType(typeof(ZeroOrderMissingValuePolicy))]
    [KnownType(typeof(FirstOrderMissingValuePolicy))]
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