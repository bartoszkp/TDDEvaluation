using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            Random random = new Random();

            var id = client.Add(new Signal()
            {
                DataType = DataType.Double,
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "signal" + random.Next(100)} }
            }).Id.Value;

            client.SetData(id, new Datum[] {
              new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
              new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
              new Datum() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 } });

            var result = client.GetData(id, new DateTime(2000, 3, 1), new DateTime(2000, 1, 1));

            Console.WriteLine(result.Length);
            
            Console.ReadKey();
        }
    }
}
