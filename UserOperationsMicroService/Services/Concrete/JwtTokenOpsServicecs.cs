using UserOperationsMicroService.Configuration.Concrete;
using UserOperationsMicroService.Configuration.Interface;
using Shared.Models;
using UserOperationsMicroService.Services.Interface;
using UserOperationsMicroService.Utils;

namespace UserOperationsMicroService.Services.Concrete
{
    public class JwtTokenOpsServicecs : IJwtTokenOpsServicecs
    {
        private readonly JwtTokenOps jwtTokenOps;
        private readonly IAppConfiguration _appConfig;

        public JwtTokenOpsServicecs(IAppConfiguration appConfig)
        {
            _appConfig = appConfig;
            jwtTokenOps = new JwtTokenOps(_appConfig);
        }

        public string GenerateToken(User user)
        {
            return jwtTokenOps.GenerateToken(user);
        }

        public int getUserIdFromToken(string jwtToken)
        {
            return jwtTokenOps.getUserIdFromToken(jwtToken);
        }

        public string VerifyToken(string jwtToken)
        {
            return jwtTokenOps.VerifyToken(jwtToken);
        }
    }
}
