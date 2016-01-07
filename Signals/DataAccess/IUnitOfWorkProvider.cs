namespace DataAccess
{
    public interface IUnitOfWorkProvider
    {
        UnitOfWork OpenUnitOfWork();

        UnitOfWork OpenReadOnlyUnitOfWork();
    }
}