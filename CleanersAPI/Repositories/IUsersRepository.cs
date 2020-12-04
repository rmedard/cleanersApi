using CleanersAPI.Models;

namespace CleanersAPI.Repositories
{
    public interface IUsersRepository : ICleanersRepository<User>
    {
        void CreateRoleUser(RoleUser roleUser);

        Role GetRoleByName(RoleName roleName);
    }
}