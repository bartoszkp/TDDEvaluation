﻿using System;
using ExampleSignalClient.Signals;
using System.Linq;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");
            
            /*client.Add(new Signal() { Path = new Path() { Components = new[] { "s0" } }, Granularity = Granularity.Month, DataType = DataType.Double });
            client.Add(new Signal() { Path = new Path() { Components = new[] { "root", "s1" } }, Granularity = Granularity.Month, DataType = DataType.Double });
            client.Add(new Signal() { Path = new Path() { Components = new[] { "root", "s1", "s2" } }, Granularity = Granularity.Month, DataType = DataType.Double });
            client.Add(new Signal() { Path = new Path() { Components = new[] { "root", "podkatalog", "s3" } }, Granularity = Granularity.Month, DataType = DataType.Double });
            client.Add(new Signal() { Path = new Path() { Components = new[] { "root", "podkatalog", "podpodkatalog", "s4" } }, Granularity = Granularity.Month, DataType = DataType.Double });
            client.Add(new Signal() { Path = new Path() { Components = new[] { "root", "podkatalog2", "s5" } }, Granularity = Granularity.Month, DataType = DataType.Double });
            */
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
