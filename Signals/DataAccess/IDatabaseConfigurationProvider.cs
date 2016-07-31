namespace DataAccess
{
    public interface IDatabaseConfigurationProvider
    {
        bool UseInMemoryDatabase { get; }
    }
}
