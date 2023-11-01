using  UserOperationsMicroService.Services.Interface;
using UserOperationsMicroService.Utils;

namespace UserOperationsMicroService.Services.Concrete
{
    public class PasswordComputationService : IPasswordComputationService
    {
        private readonly PasswordComputation pwdGenerator;

        public PasswordComputationService()
        {
            pwdGenerator = new PasswordComputation();
        }
        public string ComputePasswordHash(string password)
        {
            return pwdGenerator.ComputePasswordHash(password);
        }

        public bool VerifyPassword(string password, string storedHashedPassword)
        {
            return pwdGenerator.VerifyPassword(password, storedHashedPassword);
        }
    }
}
