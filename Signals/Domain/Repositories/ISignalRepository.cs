namespace Domain.Repositories
{
    public interface ISignalRepository
    {
        Signal Get(Path path);

        Signal Add(Signal signal);

        void Remove(Path path);
    }
}
