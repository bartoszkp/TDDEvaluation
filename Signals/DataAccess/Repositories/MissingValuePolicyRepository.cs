using Domain.MissingValuePolicy;

namespace DataAccess.Repositories
{
    public class MissingValuePolicyRepository : RepositoryBase, Domain.Repositories.IMissingValuePolicyRepository
    {
        public MissingValuePolicyRepository(ISessionProvider sessionProvider)
            : base(sessionProvider)
        {
        }

        public void Delete(MissingValuePolicy missingValuePolicy)
        {
            Session.Delete(missingValuePolicy);
        }
    }
}
