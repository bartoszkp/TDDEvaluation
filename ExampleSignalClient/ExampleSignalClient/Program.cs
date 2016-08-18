using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            /*
            var signals = new[] {
                new Signal()
                {
                    DataType = DataType.String,
                    Granularity = Granularity.Month,
                    Path = new Path() { Components = new[] { "root", "s1"} }
                },
                new Signal()
                {
                    DataType = DataType.String,
                    Granularity = Granularity.Month,
                    Path = new Path() { Components = new[] { "root", "podkatalog", "s2" } }
                },
                new Signal()
                {
                    DataType = DataType.String,
                    Granularity = Granularity.Month,
                    Path = new Path() { Components = new[] { "root", "podkatalog", "s3" } }
                },
                new Signal()
                {
                    DataType = DataType.String,
                    Granularity = Granularity.Month,
                    Path = new Path() { Components = new[] { "root", "podkatalog", "podpodkatalog", "s4" } }
                },
                new Signal()
                {
                    DataType = DataType.String,
                    Granularity = Granularity.Month,
                    Path = new Path() { Components = new[] { "root", "podkatalog2", "s5" } }
                },
            };

            foreach (var sig in signals)
                client.Add(sig);
            */

            var result = client.GetPathEntry(new Path() { Components = new[] { "root" } });

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
