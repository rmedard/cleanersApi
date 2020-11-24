using CleanersAPI.Models;
using CleanersAPI.Repositories;

namespace CleanersAPI.Services.Impl
{
    public class UsersService : CleanersService<User>, IUsersService 
    {
        private readonly IUsersRepository _usersRepository;

        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        protected override ICleanersRepository<User> GetRepository()
        {
            return _usersRepository;
        }
    }
}