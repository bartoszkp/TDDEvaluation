﻿using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {

        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var signal1 = client.Add(new Signal()
            {
                DataType = DataType.Boolean,
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "cycle", "s1i2gna1l1" } }
            });
            var signal2 = client.Add(new Signal()
            {
                DataType = DataType.Boolean,
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "cycle", "si1gn1a2l2" } }
            });
            var signal3 = client.Add(new Signal()
            {
                DataType = DataType.Boolean,
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "cycle", "si1g1n2al3" } }
            });

             client.SetMissingValuePolicy(
                    signal1.Id.Value,
                    new ShadowMissingValuePolicy()
                    {
                        DataType = DataType.Boolean,
                        ShadowSignal = signal2
                    });
            client.SetMissingValuePolicy(
                    signal2.Id.Value,
                    new ShadowMissingValuePolicy()
                    {
                        DataType = DataType.Boolean,
                        ShadowSignal = signal3
                    });

            try
            {
                client.SetMissingValuePolicy(
                      signal3.Id.Value,
                      new ShadowMissingValuePolicy()
                      {
                          DataType = DataType.Boolean,
                          ShadowSignal = signal1
                      });
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to assign");
            }

            Console.ReadKey();



        }
    }
}
