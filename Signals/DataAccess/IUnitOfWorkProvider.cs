namespace DataAccess
{
    public interface IUnitOfWorkProvider
    {
        UnitOfWorkBase CurrentUnitOfWork { get; }

        UnitOfWorkBase OpenUnitOfWork();

        UnitOfWorkBase OpenReadOnlyUnitOfWork();
    }
}