using CleanersAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace CleanersAPI.Helpers.Auth
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public string RoleName { get; private set; }

        public RoleRequirement(string roleName)
        {
            RoleName = roleName;
        }
    }
}