using Shared.Models;
using Shared.Services.Interface;
using Shared.Repos.Interface;

namespace Shared.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;    
        }
        public async Task<User> GetUserById(int id)
        {
            return await _userRepo.Get(id);
        }
    }
}


