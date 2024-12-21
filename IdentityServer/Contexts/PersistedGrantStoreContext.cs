using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Contexts
{
    public class PersistedGrantStoreContext : PersistedGrantDbContext<PersistedGrantStoreContext>
    {
        public PersistedGrantStoreContext(DbContextOptions<PersistedGrantStoreContext> options) : base(options)
        {
        }
    }

    public class IDSConfigurationDbContext : ConfigurationDbContext<IDSConfigurationDbContext>
    {
        public IDSConfigurationDbContext(DbContextOptions<IDSConfigurationDbContext> options) : base(options)
        {
        }
    }
}
