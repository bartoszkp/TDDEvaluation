namespace Domain.Repositories
{
    public interface IMissingValuePolicyRepository
    {
        void Set(Signal signal, MissingValuePolicy.MissingValuePolicyBase missingValuePolicy);

        MissingValuePolicy.MissingValuePolicyBase Get(Signal signal);
    }
}
