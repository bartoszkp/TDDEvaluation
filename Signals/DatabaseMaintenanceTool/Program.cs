using System;
using System.IO;
using System.Configuration;

namespace DatabaseMaintenanceTool
{
    public class Program
    {
        private const string DataDirectoryDataKey = "DataDirectory";

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

                CreateDatabaseIfNotExists();

                DatabaseMaintenance.DatabaseMaintenance dm = new DatabaseMaintenance.DatabaseMaintenance(new DataAccess.UnitOfWorkProvider());

                dm.RebuildDatabase();

                Console.WriteLine("Done.");
            }
        }

        private static void SetupDataDirectory()
        {
            var relativeDataDirectory = Properties.Settings.Default["RelativeDataDirectory"] as string;
            var absoluteDataDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativeDataDirectory));
            AppDomain.CurrentDomain.SetData(DataDirectoryDataKey, absoluteDataDirectory);
        }

        private static void CreateDatabaseIfNotExists()
        {
            string databaseName = "Signals";
            string fileName = Path.Combine(AppDomain.CurrentDomain.GetData(DataDirectoryDataKey).ToString(), databaseName + ".mdf");

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
                        = string.Format("CREATE DATABASE {0} ON PRIMARY (NAME={0}, FILENAME='{1}')", databaseName, fileName);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
