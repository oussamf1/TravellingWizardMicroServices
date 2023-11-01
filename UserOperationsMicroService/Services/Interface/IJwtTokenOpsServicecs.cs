using Shared.Models;

namespace UserOperationsMicroService.Services.Interface
{
    public interface IJwtTokenOpsServicecs
    {
        public string GenerateToken(User user);

        public string VerifyToken(string jwtToken);

        public int getUserIdFromToken(string jwtToken);


    }
}
