using System;
using System.IO;

namespace DatabaseMaintenanceTool
{
    class Program
    {
        private static void setupDataDirectory()
        {
            var relativeDataDirectory = Properties.Settings.Default["RelativeDataDirectory"] as string;
            var absoluteDataDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativeDataDirectory));
            AppDomain.CurrentDomain.SetData("DataDirectory", absoluteDataDirectory);
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No arguments");
                return;
            }

            if (args[0] == "rebuild")
            {
                Console.WriteLine("Rebuilding database...");

                setupDataDirectory();

                DatabaseMaintenance.DatabaseMaintenance dm = new DatabaseMaintenance.DatabaseMaintenance(new DataAccess.UnitOfWorkProvider());

                dm.RebuildDatabase();

                Console.WriteLine("Done.");
            }
        }
    }
}
