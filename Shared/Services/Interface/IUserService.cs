using Shared.Models;

namespace Shared.Services.Interface
{
    public interface IUserService
    {
        public Task<User> GetUserById(int id);
        

    }
}
