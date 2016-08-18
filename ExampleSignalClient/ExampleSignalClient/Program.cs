using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            /*Path[] paths = new[] { 
                                                new Path() { Components = new[] { "root", "s1" } },
                                                new Path() { Components = new[] { "root", "s1","s2" } } };

            Random random = new Random();

            for (int i = 0; i < paths.Length; i++)
            {
                client.Add(new Signal()
                {
                    DataType = DataType.Decimal,
                    Granularity = Granularity.Month,
                    Path = paths[i]
                });
            }
            */

            var result = client.GetPathEntry(new Path() { Components = new[] { "root","s1" } });

            Console.WriteLine("Sygnały w 'root':");
            foreach (var r in result.Signals)
            {
                Console.WriteLine(string.Join("/", r.Path.Components) + ", " + r.Id);
            }
            Console.WriteLine("Ścieżki podrzędne w 'root':");
            foreach (var s in result.SubPaths)
            {
                Console.WriteLine(string.Join("/", s.Components));
            }


            Console.ReadKey();


        }
    }
}
