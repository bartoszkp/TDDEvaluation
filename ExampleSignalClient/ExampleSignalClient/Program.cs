﻿using System;
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
                DataType = DataType.Integer,
                Granularity = Granularity.Minute,
                Path = new Path() { Components = new[] { "FirstOrderTests" } }
            }).Id.Value;

            client.SetMissingValuePolicy(id, new FirstOrderMissingValuePolicy() { DataType = DataType.Integer });

            client.SetData(id, new Datum[]
            {
                new Datum() { Timestamp = new DateTime(2000, 1, 1, 1, 2, 0), Value = 10, Quality = Quality.Good },
                new Datum() { Timestamp = new DateTime(2000, 1, 1, 1, 5 ,0), Value = 30, Quality = Quality.Good }
            });

            var result = client.GetData(id, new DateTime(2000, 1, 1, 1, 1, 0), new DateTime(2000, 1, 1, 1, 10, 0));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();

        }
    }
}
