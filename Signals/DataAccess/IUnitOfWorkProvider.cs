namespace DataAccess
{
    public interface IUnitOfWorkProvider
    {
        UnitOfWork CurrentUnitOfWork { get; }

        UnitOfWork OpenUnitOfWork();

        UnitOfWork OpenReadOnlyUnitOfWork();
    }
}