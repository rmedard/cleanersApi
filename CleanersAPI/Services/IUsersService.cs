using CleanersAPI.Models;

namespace CleanersAPI.Services
{
    public interface IUsersService : ICleanersService<User>
    {
        Role GetRoleByName(RoleName roleName);
    }
}