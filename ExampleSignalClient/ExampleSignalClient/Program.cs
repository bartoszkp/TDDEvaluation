using System;
using ExampleSignalClient.Signals;

namespace ExampleSignalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            SignalsWebServiceClient client = new SignalsWebServiceClient("BasicHttpBinding_ISignalsWebService");
            /*
                        var id = client.Add(new Signal()
                        {
                            DataType = DataType.Decimal,
                            Granularity = Granularity.Month,
                            Path = new Path() { Components = new [] {"x","y"}}
                        }).Id;



                        client.SetData(id.GetValueOrDefault(), new Datum[] {
                            new Datum() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                            new Datum() { Quality = Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                            new Datum() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 } });

                        var result = client.GetData(id.GetValueOrDefault(), new DateTime(2000, 1, 1), new DateTime(2000, 1, 1));

                        foreach (var d in result)
                        {
                            Console.WriteLine(d.Timestamp.ToString() + ": " + d.Value.ToString() + " (" + d.Quality.ToString() + ")");
                        }

                        Console.ReadKey();
                       */
/*
            Path[] paths = new[] { new Path() { Components = new[] { "root", "s1" } },
                                               new Path() { Components = new[] { "s0" } },
                                               new Path() { Components = new[] { "root", "podkatalog","s2" } },
                                               new Path() { Components = new[] { "root", "podkatalog2","s3" } },
                                               new Path() { Components = new[] { "root", "podkatalog","podkatalog","s4" } } };

            Random random = new Random();

            for (int i = 0; i < paths.Length; i++)
            {
                client.Add(new Signal()
                {
                    DataType = DataType.Decimal,
                    Granularity = Granularity.Month,
                    Path = paths[i]
                });
            }

    */
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
