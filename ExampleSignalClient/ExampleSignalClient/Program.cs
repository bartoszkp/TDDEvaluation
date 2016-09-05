using System;
using ExampleSignalClient.Signals;
using System.Linq;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var id = 1;
            var bool_month_id = client.Add(new Signal()
            {
                DataType = DataType.Boolean,
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "bool", "month", id.ToString() } }
            }).Id.Value;
            var bool_second_id = client.Add(new Signal()
            {
                DataType = DataType.Boolean,
                Granularity = Granularity.Second,
                Path = new Path() { Components = new[] { "bool", "second", id.ToString() } }
            }).Id.Value;
            var decimal_month_id = client.Add(new Signal()
            {
                DataType = DataType.Decimal,
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "decimal", "month", id.ToString() } }
            }).Id.Value;

            var bool_month_shadow = client.Add(new Signal()
            {
                DataType = DataType.Boolean,
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "shadows", "bool", "month", id.ToString() } }
            });

            try
            {
                client.SetMissingValuePolicy(
                    bool_second_id,
                    new ShadowMissingValuePolicy()
                    {
                        DataType = DataType.Boolean,
                        ShadowSignal = bool_month_shadow
                    });
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to assign");
            }

            try
            {
                client.SetMissingValuePolicy(
                    decimal_month_id,
                    new ShadowMissingValuePolicy()
                    {
                        DataType = DataType.Decimal,
                        ShadowSignal = bool_month_shadow
                    });
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to assign");
            }

            client.SetMissingValuePolicy(
                bool_month_id,
                new ShadowMissingValuePolicy()
                {
                    DataType = DataType.Boolean,
                    ShadowSignal = bool_month_shadow
                });

            var mvp = client.GetMissingValuePolicy(bool_month_id);

            Console.WriteLine(mvp.GetType().ToString());
            Console.WriteLine(string.Join(",", ((ShadowMissingValuePolicy)mvp).ShadowSignal.Path.Components));

            Console.ReadKey();

        }
    }
}
