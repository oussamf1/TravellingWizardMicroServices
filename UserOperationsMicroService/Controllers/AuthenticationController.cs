
using Microsoft.AspNetCore.Mvc;
using Shared.Services.Interface;
using Shared.Configuration.Interface;
using UserOperationsMicroService.DTOs;
using UserOperationsMicroService.Services.Interface;
using UserOperationsMicroService.Utils;

namespace UserOperationsMicroService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenOpsServicecs _jwtTokenOpsServicecs;
        private readonly IUserActionsService _userActionsService;
        private readonly IAppConfiguration _appConfiguration;
        public AuthenticationController(IAppConfiguration appConfiguration,IUserActionsService userActionsService, IJwtTokenOpsServicecs jwtTokenOpsServicecs)
        {
            _userActionsService = userActionsService;

            _jwtTokenOpsServicecs = jwtTokenOpsServicecs;

            _appConfiguration = appConfiguration;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register (UserRegistrationDTO userResgistrationDto)
        {
            try
            {
                var registrationResult = await _userActionsService.RegisterUser(userResgistrationDto);

                if(registrationResult.isRegistered == false)
                {
                    var response = new ObjectResult(registrationResult)
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };

                    return response;
                }
                else
                {
                    var response = new ObjectResult(registrationResult)
                    {
                        StatusCode = StatusCodes.Status200OK,
                    };

                    return response;
                }
             
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserAuthenticationDTO userAuthenticationDTO)
        {
            try
            {
                var authenticationResult = await _userActionsService.Login(userAuthenticationDTO);

                if (authenticationResult.isAuthenticated)
                {
                    var response = new ObjectResult(authenticationResult)
                    {
                        StatusCode = StatusCodes.Status200OK
                    };

                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Path = "/",
                        IsEssential = true,
                        SameSite = SameSiteMode.None,
                        
                    };

                    Response.Cookies.Append("jwtToken", authenticationResult.Token, cookieOptions);

                    return response;

                }
                else
                {
                    var response = new ObjectResult(authenticationResult)
                    {
                        

                    };

                    return response;

                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }

        [HttpGet("confirmAccount")]
        public async Task<IActionResult> ConfirmAccount([FromQuery] string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Invalid or missing confirmation token.");
            }

            var result = await _userActionsService.ConfirmUserEmail(token);

            if (result)
            {
                return Redirect(_appConfiguration.FrontEndUrl);
            }
            else
            {
                return BadRequest("Account confirmation failed.");
            }
        }


        [HttpGet("isLoggedIn")]
         public async Task<IActionResult> IsLoggedIn()
         {
            if (Request.Cookies.TryGetValue("jwtToken", out string jwtToken))
            {
                var result = await _userActionsService.IsLoggedIn(jwtToken);

                return Ok(new { IsLoggedIn = true, user = result });
            }
            else
            {
                return Ok(new { IsLoggedIn = false });
            }

        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            
            Response.Cookies.Delete("jwtToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Path = "/",
                IsEssential = true,
                SameSite = SameSiteMode.None
            });

            return Ok("Cookie cleared successfully");
        }
    }

}


