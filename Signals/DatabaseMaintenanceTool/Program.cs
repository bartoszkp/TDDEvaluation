using System;
using System.IO;

namespace DatabaseMaintenanceTool
{
    public class Program
    {
        private static void SetupDataDirectory()
        {
            var relativeDataDirectory = Properties.Settings.Default["RelativeDataDirectory"] as string;
            var absoluteDataDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativeDataDirectory));
            AppDomain.CurrentDomain.SetData("DataDirectory", absoluteDataDirectory);
        }

        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No arguments");
                return;
            }

            if (args[0] == "rebuild")
            {
                Console.WriteLine("Rebuilding database...");

                SetupDataDirectory();

                DatabaseMaintenance.DatabaseMaintenance dm = new DatabaseMaintenance.DatabaseMaintenance(new DataAccess.UnitOfWorkProvider());

                dm.RebuildDatabase();

                Console.WriteLine("Done.");
            }
        }
    }
}
