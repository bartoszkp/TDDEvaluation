using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var id = client.Add(new Signal() { Path = new Path() { Components = new[] { string.Empty } } })
                .Id.Value;

            var result = client.GetById(id);

            if (result != null)
            {
                Console.WriteLine("Sygnał poprawnie utworzony");
            }
            else
            {
                Console.WriteLine("Błąd - nie udało się utworzyć sygnału");
            }

            client.Delete(id);

            result = client.GetById(id);

            if (result == null)
            {
                Console.WriteLine("Sygnał poprawnie skasowany");
            }
            else
            {
                Console.WriteLine("Błąd - sygnał nadal istnieje");
            }

            Console.ReadKey();
        }
    }
}
