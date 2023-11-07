using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Configuration.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ISystemClock = Microsoft.AspNetCore.Authentication.ISystemClock;

namespace Shared.Authentication
{
    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        public string HeaderName { get; set; }

    }

    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private readonly IAppConfiguration _appConfiguration;

        public ApiKeyAuthenticationHandler(
            IAppConfiguration appConfiguration,
            IOptionsMonitor<ApiKeyAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)

            : base(options, logger, encoder, clock)
        {
            _appConfiguration = appConfiguration;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Context.Request.Headers.TryGetValue("ApiKey", out var apiKeyHeaderValue))
            {
                var apiKey = apiKeyHeaderValue.ToString();

                if (apiKey == _appConfiguration.ApiKey)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, "Service"),
                    };

                    var identity = new ClaimsIdentity(claims, "ApiKeyAuthentication");
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);
                    return Task.FromResult(AuthenticateResult.Success(ticket));
                }
            }

            return Task.FromResult(AuthenticateResult.Fail("Invalid API key."));
        }
    }
}

