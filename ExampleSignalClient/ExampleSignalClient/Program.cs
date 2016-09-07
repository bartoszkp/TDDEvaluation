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
                Path = new Path() { Components = new[] { "DeleteTests2" } }
            }).Id.Value;

            client.SetData(id, new Datum[]
            {
    new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 1m },
            });

            client.Delete(id);

            try
            {
                client.GetData(id, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1));
            }
            catch (Exception e)
            {
                Console.WriteLine("Brak danych usuniętego sygnału");
            }

            Console.ReadKey();

        }
    }
}
