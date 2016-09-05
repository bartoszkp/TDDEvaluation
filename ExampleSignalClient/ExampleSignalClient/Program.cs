using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var id = client.Add(new Signal()
            {
                DataType = DataType.Decimal,
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "FirstOrderTests" } }
            }).Id.Value;

            client.SetMissingValuePolicy(id, new FirstOrderMissingValuePolicy() { DataType = DataType.Decimal });

            var beginTimestamp = new DateTime(2018, 1, 1);
            var endTimestamp = beginTimestamp.AddMonths(5);

            client.SetData(id, new Datum[]
            {
                new Datum() { Quality = Quality.Good, Value = 10m, Timestamp = beginTimestamp.AddMonths(-1) },
                new Datum() { Quality = Quality.Fair, Value = 20m, Timestamp = beginTimestamp.AddMonths(-2) },
                new Datum() { Quality = Quality.Poor, Value = 80m, Timestamp = endTimestamp.AddMonths(1) },
            });

            var result = client.GetData(id, beginTimestamp, endTimestamp);

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();
        }
    }
}

