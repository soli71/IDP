using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Entities;

public class User:IdentityUser<int>
{
}
public class Role : IdentityRole<int> { }

public class UserClaim : IdentityUserClaim<int> { }

public class UserRole : IdentityUserRole<int> { }

public class UserLogin : IdentityUserLogin<int> { }

public class RoleClaim : IdentityRoleClaim<int> { }

public class UserToken : IdentityUserToken<int> { }


