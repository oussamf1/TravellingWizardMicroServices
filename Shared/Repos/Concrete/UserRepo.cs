using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Shared.Repos.Interface;
using Shared.Data;

namespace Shared.Repos.Concrete
{
    public class UserRepo : GenericRepo<User,ApplicationDbContext>,IUserRepo
    {
        private readonly ApplicationDbContext _dbContext;
        public UserRepo(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<bool> ConfirmUserEmail(int Id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == Id);

            user.IsEmailConfirmed = true;

            bool isSaved = _dbContext.SaveChanges() > 0;

            return isSaved;

        }

        public async Task<User> GetByEmail(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Email == email);

            return user;
        }

        public async Task<User> GetByToken(string token)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.ConfirmationToken == token);

            return user;
        }

        public Task<User> GetUserById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
