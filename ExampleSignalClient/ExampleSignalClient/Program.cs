using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            client.Add(new Signals.Signal()
            {
                DataType = DataType.Double,
                Granularity = Granularity.Week
            });

            var data = new Datum[]
            {
                new Datum() { Quality = Quality.Bad, Value = (double)0, Timestamp = new DateTime(2016, 8, 22, 0, 0, 0) }
            };

            client.SetData(1, data);

            var result = client.GetData(1, new DateTime(2016, 8, 22, 0, 0, 0), new DateTime(2016, 8, 29, 0, 0, 0));

            foreach(var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();
        }
    }
}
