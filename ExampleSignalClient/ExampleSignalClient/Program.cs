﻿using System;
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
                DataType = DataType.Decimal,
                Granularity = Granularity.Month
            });

            client.SetMissingValuePolicy(1, new FirstOrderMissingValuePolicy() { DataType = DataType.Decimal });

            client.SetData(1, new Datum[]
            {
                new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (decimal)1 },
                new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (decimal)2 }
            });

            var result = client.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp + ": " + d.Value + " (" + d.Quality + ")");
            }

            Console.ReadKey();
        }
    }
}
