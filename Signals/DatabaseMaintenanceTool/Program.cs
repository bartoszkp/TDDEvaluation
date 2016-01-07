using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMaintenanceTool
{
    class Program
    {
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

                DatabaseMaintenance.DatabaseMaintenance dm = new DatabaseMaintenance.DatabaseMaintenance(new DataAccess.UnitOfWorkProvider());

                dm.RebuildDatabase();

                Console.WriteLine("Done.");
            }
        }
    }
}
