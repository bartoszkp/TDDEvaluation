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

            var newSignal = new Signal()
            {
                DataType = DataType.String,
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "root", "stringSignal2" } }
            };

            var id = client.Add(newSignal).Id.Value;

            client.SetData(id, new[] { new Datum() { Quality = Quality.Good, Value = null, Timestamp = new DateTime(2000, 1, 1) } });

            var result = client.GetData(id, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Quality);
                Console.WriteLine(d.Value ?? "null");
            }

            Console.ReadKey();

        }
    }
}
