using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using static Shared.Authentication.JwtCookieAuthentication.JwtCookieAuthenticationHandler;

namespace Shared.Authentication
{
    public class JwtCookieAuthentication
    {
        public class JwtCookieAuthenticationHandler : AuthenticationHandler<JwtCookieAuthenticationOptions>
        {
            private readonly IJwtTokenOpsServicecs _jwtTokenOpsServicecs;
            public class JwtCookieAuthenticationOptions : AuthenticationSchemeOptions
            {
                public string CookieName { get; set; }
            }
            public JwtCookieAuthenticationHandler(
                IOptionsMonitor<JwtCookieAuthenticationOptions> options,
                IJwtTokenOpsServicecs jwtTokenOpsServicecs,
                ILoggerFactory logger,
                UrlEncoder encoder,
                ISystemClock clock)
                : base(options, logger, encoder, clock)
            {
                _jwtTokenOpsServicecs = jwtTokenOpsServicecs;
            }

            protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
            {
                if (!Request.Cookies.TryGetValue("jwtToken", out var token))
                {
                    return AuthenticateResult.NoResult();
                }
                if (IsValidToken(token))
                {
                    var claims = _jwtTokenOpsServicecs.GetClaimsFromToken(token);
                    var identity = new ClaimsIdentity(claims, "JwtCookieAuthentication");
                    var claimsPrincipal = new ClaimsPrincipal(identity);
                    var authenticationTicket = new AuthenticationTicket(claimsPrincipal, "JwtCookieAuthentication");

                    return AuthenticateResult.Success(authenticationTicket);
                }

                return AuthenticateResult.Fail("Invalid token");
            }
            private bool IsValidToken(string token)
            {
                return true;
            }
        }

    }
}
