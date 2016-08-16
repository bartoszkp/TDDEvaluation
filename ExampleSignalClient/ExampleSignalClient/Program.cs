using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");

            //var newSignal1 = new Signal()
            //{
            //    Id = null,
            //    DataType = DataType.String,
            //    Granularity = Granularity.Month,
            //    Path = new Path() { Components = new[] { "z", "d" } },
            //};

            //var newSignal2 = new Signal()
            //{
            //    Id = null,
            //    DataType = DataType.Double,
            //    Granularity = Granularity.Month,
            //    Path = new Path { Components = new[] { "x", "y" } }
            //};

            //var signalId1 = client.Add(newSignal1).Id;
            //var signalId2 = client.Add(newSignal2).Id;

            var mvp1 = new Signals.SpecificValueMissingValuePolicy() { DataType = DataType.String, Quality = Quality.Fair, Value = "x" };
            var mvp2 = new Signals.NoneQualityMissingValuePolicy() { DataType = DataType.Double};

            client.SetMissingValuePolicy(1, mvp1);
            client.SetMissingValuePolicy(2, mvp2);

            var result1 = client.GetMissingValuePolicy(1) as Signals.SpecificValueMissingValuePolicy;
            var result2 = client.GetMissingValuePolicy(2) as Signals.NoneQualityMissingValuePolicy;

            Console.WriteLine(result1.Signal.Id.Value);
            Console.WriteLine(result2.Signal.Id.Value);
            Console.WriteLine(result1.DataType);
            Console.WriteLine(result1.Quality);
            Console.WriteLine(result1.Value);
            Console.ReadKey();
        }
    }
}
