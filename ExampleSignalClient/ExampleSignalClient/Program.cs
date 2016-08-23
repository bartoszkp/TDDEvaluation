using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var signal = client.Add(new Signal()
            {
                DataType = DataType.Integer,
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "example", "path7" } },
            });

            var data = new Datum[]
            {
                new Datum() { Quality = Quality.Bad, Value = 0, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0) }
            };

            client.SetData(signal.Id.Value, data);

            var result = client.GetData(signal.Id.Value, new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 3, 1, 0, 0, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();


        }
    }
}
