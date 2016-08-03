﻿using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var signal = new Signal()
            {
                   DataType = DataType.Double,
                    Granularity = Granularity.Month,
                    Path =  new Path() { Components = new[] { "root", "signal" } }
            };

            var id = client.Add(signal).Id.Value;

            var mvp = new Signals.SpecificValueMissingValuePolicy() { DataType = DataType.Double, Quality = Quality.Fair, Value = (double)1.5 };

            client.SetMissingValuePolicy(id, mvp);

            var result = client.GetMissingValuePolicy(id) as Signals.SpecificValueMissingValuePolicy;

            Console.WriteLine(result.Signal.Id.Value);
            Console.WriteLine(result.DataType);
            Console.WriteLine(result.Quality);
            Console.WriteLine(result.Value);
            Console.WriteLine("end");
            Console.ReadKey();
        }
    }
}
