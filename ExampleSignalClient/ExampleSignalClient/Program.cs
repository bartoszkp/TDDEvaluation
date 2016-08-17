using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            //var signal = client.Add(new Signal()
            //{
            //    DataType = DataType.Integer,
            //    Granularity = Granularity.Month,
            //    Path = new Path() { Components = new[] { "signal" } }
            //});

            client.SetData(1, new[] { new Datum() { Quality = Quality.Good, Value = 1, Timestamp = new DateTime(2000, 1, 1) } });

            var result = client.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();

        }
    }
}
