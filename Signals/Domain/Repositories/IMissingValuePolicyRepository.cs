namespace Domain.Repositories
{
    public interface IMissingValuePolicyRepository
    {
        void Add(MissingValuePolicy.MissingValuePolicy missingValuePolicy);

        void Delete(MissingValuePolicy.MissingValuePolicy missingValuePolicy);

        Domain.MissingValuePolicy.MissingValuePolicy Get(int signalId);
    }
}
