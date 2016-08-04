using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            //var newSignal = new Signal()
            //{
            //    DataType = DataType.Double,
            //    Granularity = Granularity.Month,
            //   Path = new Path() { Components = new[] { "", "" } }
            //};

            //client.Add(newSignal);

            //var result = client.Get(new Path() { Components = new[] { "root1", "signal1" } });

            //Console.WriteLine(result.Id.Value);
            //Console.WriteLine(result.DataType);
            //Console.WriteLine(result.Granularity);
            //Console.WriteLine(string.Join("/", result.Path.Components));


            var mvp = new Signals.NoneQualityMissingValuePolicy() { DataType = DataType.Double };
            client.SetMissingValuePolicy(1, mvp);

            //var result = client.GetMissingValuePolicy(1) as Signals.NoneQualityMissingValuePolicy;

            //Console.WriteLine(result.Signal.Id.Value);
            //Console.WriteLine(result.DataType);
            //Console.ReadKey();


            Console.WriteLine("good");

            Console.ReadKey();
        }
    }
}
