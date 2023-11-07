using Shared.Configuration.Interface;
using Shared.Models;
using Shared.Services.Interface;
using Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services.Concrete
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

        public bool IsValidUser(string jwtToken)
        {
            return jwtTokenOps.IsValidUser(jwtToken);
        }

        public string VerifyToken(string jwtToken)
        {
            return jwtTokenOps.VerifyToken(jwtToken);
        }

        public List<Claim> GetClaimsFromToken(string jwtToken)
        {
            return jwtTokenOps.GetClaimsFromToken(jwtToken);
        }
    }
}
