using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");
            
            //client.Add(new Signals.Signal()
            //{
            //    DataType = DataType.Boolean,
            //    Granularity = Granularity.Second,
            //    Path = new Signals.Path() { Components = new[] { "root", "s1" } }
            //});

            //client.Add(new Signals.Signal()
            //{
            //    DataType = DataType.Boolean,
            //    Granularity = Granularity.Second,
            //    Path = new Signals.Path() { Components = new[] { "root", "s1", "s2" } }
            //});

            //client.Add(new Signals.Signal()
            //{
            //    DataType = DataType.Boolean,
            //    Granularity = Granularity.Second,
            //    Path = new Signals.Path() { Components = new[] { "root", "podkatalog", "s3" } }
            //});

            //client.Add(new Signals.Signal()
            //{
            //    DataType = DataType.Boolean,
            //    Granularity = Granularity.Second,
            //    Path = new Signals.Path() { Components = new[] { "root", "podkatalog", "podpodkatalog", "s4" } }
            //});

            //client.Add(new Signals.Signal()
            //{
            //    DataType = DataType.Boolean,
            //    Granularity = Granularity.Second,
            //    Path = new Signals.Path() { Components = new[] { "root", "podkatalog2", "s5" } }
            //});

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

            var result2 = client.GetPathEntry(new Path() { Components = new[] { "root", "s1" } });
            Console.WriteLine("Sygnały w 'root/s1':");
            foreach(var s in result2.Signals)
            {
                Console.WriteLine(string.Join("/", s.Path.Components) + ", " + s.Id);
            }
            Console.ReadKey();
        }
    }
}
