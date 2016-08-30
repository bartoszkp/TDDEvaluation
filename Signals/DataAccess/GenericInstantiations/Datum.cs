using Domain;

namespace DataAccess.GenericInstantiations
{
    public class DatumBoolean : Domain.Datum<bool>
    {
    }

    public class DatumInteger : Domain.Datum<int>
    {
        public override int GetFirstOrderValueToAdd(int value, long timestampDifferenceOlderNewerDatum, long timestampDifferenceOlderMissingDatum)
        {
            return value + (this.Value - value) / (int)timestampDifferenceOlderNewerDatum * (int)timestampDifferenceOlderMissingDatum;
        }
    }

    public class DatumDouble : Domain.Datum<double>
    {
        public override double GetFirstOrderValueToAdd(double value, long timestampDifferenceOlderNewerDatum, long timestampDifferenceOlderMissingDatum)
        {
            return value + (this.Value - value) / timestampDifferenceOlderNewerDatum * timestampDifferenceOlderMissingDatum;
        }
    }

    public class DatumDecimal : Domain.Datum<decimal>
    {
        public override decimal GetFirstOrderValueToAdd(decimal value, long timestampDifferenceOlderNewerDatum, long timestampDifferenceOlderMissingDatum)
        {
            return value + (this.Value - value) / timestampDifferenceOlderNewerDatum * timestampDifferenceOlderMissingDatum;
        }
    }

    public class DatumString : Domain.Datum<string>
    {
    }
}
