﻿using System;
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
            //    Path = new Signals.Path() { Components = new[] { "s0" } }
            //});

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
            //    Path = new Signals.Path() { Components = new[] { "root", "podkatalog", "s2" } }
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
            //    Path = new Signals.Path() { Components = new[] { "root", "podkatalog", "podpodkatalog",  "s4" } }
            //});

            //client.Add(new Signals.Signal()
            //{
            //    DataType = DataType.Boolean,
            //    Granularity = Granularity.Second,
            //    Path = new Signals.Path() { Components = new[] { "root", "podkatalog2", "s5" } }
            //});

            var result = client.GetPathEntry(new Path() { Components = new[] { "roo" } });

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
