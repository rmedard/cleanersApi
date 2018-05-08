using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace CleanersAPI.Helpers.Auth
{
    public class RoleHandler : AuthorizationHandler<RoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
//            if (!context.User.HasClaim(c => c.Type == "roleName"))
//            {
//                return Task.CompletedTask;
//            }

//            if (requirement.RoleName == )
//            {
//                
//            }
            return null;
        }
    }
}