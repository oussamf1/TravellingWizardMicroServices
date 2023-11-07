using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Shared.Configuration.Interface;
using Shared.Models;
using System.Security.Claims;
using Newtonsoft.Json.Linq;

namespace UserOperationsMicroService.Utils
{
    public class JwtTokenOps
    {
        private readonly IAppConfiguration _appConfig;
        private readonly JwtSecurityTokenHandler tokenHandler;

        public JwtTokenOps(IAppConfiguration appConfig)
        {
            _appConfig = appConfig;
             tokenHandler = new JwtSecurityTokenHandler();

        }
        public string GenerateToken(User user)
        {

            var jwtSecret = Encoding.UTF8.GetBytes(_appConfig.JwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),

                    new Claim("ID", user.Id.ToString()),

                    new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),

                    new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),

                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())

                }),

                Expires = DateTime.Now.AddHours(1),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(jwtSecret),SecurityAlgorithms.HmacSha256),

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return  tokenHandler.WriteToken(token);

        }

        public string VerifyToken(string jwtToken)
        {
            var token = tokenHandler.ReadJwtToken(jwtToken);
            return token.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

        }
        public bool IsValidUser(string jwtToken)
        {
            var token = tokenHandler.ReadJwtToken(jwtToken);
            return token.Claims.FirstOrDefault(c => c.Type == "email")?.Value != null;
        }
        public int getUserIdFromToken(string jwtToken)
        {
            var token = tokenHandler.ReadJwtToken(jwtToken);
            var IdStr= token.Claims.FirstOrDefault(c => c.Type == "ID")?.Value;
            return int.Parse(IdStr);
        }
        public List<Claim> GetClaimsFromToken(string jwtToken)
        {
            var token = tokenHandler.ReadJwtToken(jwtToken);
            return token.Claims.ToList();
        }

    }
}
