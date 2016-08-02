using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var newSignal = new Signal()
            {
                DataType = DataType.Double,
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "root12", "signal12" } }
            };

            var id = client.Add(newSignal).Id.Value;

            var result = client.GetById(id);



            client.SetData(1, new Datum[] {
    new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
    new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
    new Datum() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 } });

            var result1 = client.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));

            //foreach (var d in result1)
            //{
            //    Console.WriteLine(d.Timestamp.ToString() + ": " + d.Value.ToString() + " (" + d.Quality.ToString() + ")");
            //}

            //Console.WriteLine(result.Id);
            //Console.WriteLine(result.DataType);
            //Console.WriteLine(result.Granularity);
            //Console.WriteLine(string.Join("/", result.Path.Components));
            Console.ReadKey();
        }
    }
}
