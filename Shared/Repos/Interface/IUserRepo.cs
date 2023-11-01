using Shared.Models;
using Shared.Repos.Concrete;

namespace Shared.Repos.Interface
{
    public interface IUserRepo : IGenericRepo<User>
    {
        Task<User> GetByEmail(string email);
        Task<User> GetByToken(string token);
        Task<bool> ConfirmUserEmail(int Id);
    }
}
