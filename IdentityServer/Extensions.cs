using IdentityServer.Contexts;
using IdentityServer.Entities;

namespace IdentityServer;

public static class Extensions
{
    public static void AddIDSIdentity(this IServiceCollection services)
    {
        services
            .AddIdentity<User, Role>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();
    }
}
