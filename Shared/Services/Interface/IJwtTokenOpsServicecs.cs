using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services.Interface
{
    public interface IJwtTokenOpsServicecs
    {
        public string GenerateToken(User user);

        public string VerifyToken(string jwtToken);

        public int getUserIdFromToken(string jwtToken);
        public bool IsValidUser(string jwtToken);
        public List<Claim> GetClaimsFromToken(string jwtToken);
    }
}
