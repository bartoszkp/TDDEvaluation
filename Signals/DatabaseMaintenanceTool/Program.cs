using System;
using System.Configuration;

namespace DatabaseMaintenanceTool
{
    public class Program
    {
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

                CreateDatabaseIfNotExists();

                DatabaseMaintenance.DatabaseMaintenance dm = new DatabaseMaintenance.DatabaseMaintenance(new DataAccess.UnitOfWorkProvider());

                dm.RebuildDatabase();

                Console.WriteLine("Done.");
            }
        }

        private static void CreateDatabaseIfNotExists()
        {
            string databaseName = "Signals";
            string connectionString = ConfigurationManager.ConnectionStrings["master"].ConnectionString;

            using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText
                        = string.Format("IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = '{0}') DROP DATABASE {0}", databaseName);
                    command.ExecuteNonQuery();

                    command.CommandText
                        = string.Format("CREATE DATABASE {0}", databaseName);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
