using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Api1
{
    public class DynamicAuthorization : DefaultAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions _options;
        public DynamicAuthorization(IOptions<AuthorizationOptions> options) : base(options)
        {
            _options = options.Value;
        }
        public override Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            
            var parts = policyName.Split(",");
            var claimType = parts.First();
            var claimValue = parts.Last();

            var policy = new AuthorizationPolicyBuilder()
                .RequireClaim(claimType,claimValue)
                .Build();
            
            _options.AddPolicy(policyName, policy);
            

            return base.GetPolicyAsync(policyName);
        }
    }
}
