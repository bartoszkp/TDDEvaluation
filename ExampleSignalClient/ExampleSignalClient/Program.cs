using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            client.Add(new Signal()
            {
                Path = new Path() { Components = new string[] { "s0" } }
            });
            client.Add(new Signal()
            {
                Path = new Path() { Components = new string[] { "root", "s1" } }
            });
            client.Add(new Signal()
            {
                Path = new Path() { Components = new string[] { "root", "podkatalog", "s2" } }
            });
            client.Add(new Signal()
            {
                Path = new Path() { Components = new string[] { "root", "podkatalog", "s3" } }
            });
            client.Add(new Signal()
            {
                Path = new Path() { Components = new string[] { "root", "podkatalog", "podpodkatalog", "s4" } }
            });
            client.Add(new Signal()
            {
                Path = new Path() { Components = new string[] { "root", "podkatalog2", "s5" } }
            });

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
