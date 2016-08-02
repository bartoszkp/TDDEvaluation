using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            //var newSignal = new Signal()
            //{
            //    DataType = DataType.Double,
            //    Granularity = Granularity.Month,
            //    Path = new Path() { Components = new[] { "root21", "signal1" } }
            //};

            //var id = client.Add(newSignal).Id.Value;

            //var result = client.GetById(id);

            client.SetData(1, new Datum[] {
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 3), Value = (double)2 },
          });

            var result1 = client.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 10, 2));

            foreach (var d in result1)
            {
                Console.WriteLine(d.Timestamp.ToString() + ": " + d.Value.ToString() + " (" + d.Quality.ToString() + ")");
            }

            //Console.WriteLine(result.Id);
            //Console.WriteLine(result.DataType);
            //Console.WriteLine(result.Granularity);
            //Console.WriteLine(string.Join("/", result.Path.Components));
            Console.ReadKey();
        }
    }
}
