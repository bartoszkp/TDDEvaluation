﻿using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            var newSignal = new Signal()
            {
                DataType = DataType.Integer,
                Granularity = Granularity.Month,
                Path = new Path() { Components = new[] { "root1", "defaultPolicy" } }
            };

           var res =  client.Add(newSignal);

            client.SetData(2, new Datum[] {
                         new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (int)1.5 },
                         new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 2, 1), Value = (int)1 },
                         new Datum() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 1, 1), Value = (int)2 },
                        new Datum() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 4, 1), Value = (int)2 }});

            var result = client.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));

            foreach (var d in result)
            {
                Console.WriteLine(d.Timestamp.ToString() + ": " + d.Value.ToString() + " (" + d.Quality.ToString() + ")");
            }

            Console.ReadKey();
        }
    }
}
