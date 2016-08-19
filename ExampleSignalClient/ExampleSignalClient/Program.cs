using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");


            var newSignal6 = new Signal()
            {
                DataType = DataType.Double,
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "root", "s1" } }
            };
            client.Add(newSignal6);

            var newSignal = new Signal()
            {
                DataType = DataType.Double,
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "root", "s1", "s2" } }
            };
            client.Add(newSignal);


            //var newSignal2 = new Signal()
            //{
            //    DataType = DataType.Double,
            //    Granularity = Granularity.Month,
            //    Path = new Path() { Components = new[] { "root", "podkatalog", "s2" } }
            //};
            //client.Add(newSignal2);

            //var newSignal3 = new Signal()
            //{
            //    DataType = DataType.Double,
            //    Granularity = Granularity.Month,
            //    Path = new Path() { Components = new[] { "root", "podkatalog", "s3" } }
            //};
            //client.Add(newSignal3);

            //var newSignal4 = new Signal()
            //{
            //    DataType = DataType.Double,
            //    Granularity = Granularity.Month,
            //    Path = new Path() { Components = new[] { "root", "podkatalog", "podkatalog", "s4" } }
            //};
            //client.Add(newSignal4);

            //var newSignal5 = new Signal()
            //{
            //    DataType = DataType.Double,
            //    Granularity = Granularity.Month,
            //    Path = new Path() { Components = new[] { "root", "podkatalog2", "s5" } }
            //};
            //client.Add(newSignal5);


            var result = client.GetPathEntry(new Path() { Components = new[] { "root", "s1" } });

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

