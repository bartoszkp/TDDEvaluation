namespace DataAccess
{
    public class DatabaseConfigurationProvider : IDatabaseConfigurationProvider
    {
        public bool UseInMemoryDatabase { get; private set; }

        public DatabaseConfigurationProvider(bool inMemoryDatabase)
        {
            UseInMemoryDatabase = inMemoryDatabase;
        }
    }
}