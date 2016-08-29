namespace Domain
{
    public enum Quality
    {
        None,
        Good,
        Fair,
        Poor,
        Bad,
    }

    public static class QualityExtension
    {
        public static bool IsLowerQualityThan(this Quality @this, Quality q)
        {
            return q < @this;
        }
    }
}
