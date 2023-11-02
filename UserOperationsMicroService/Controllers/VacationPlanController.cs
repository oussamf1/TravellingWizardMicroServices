using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Services.Interface;
using System.Numerics;
using System.Security.Claims;
using UserOperationsMicroService.DTOs;
using UserOperationsMicroService.Services.Concrete;
using UserOperationsMicroService.Services.Interface;

namespace UserOperationsMicroService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VacationPlanController : Controller
    {
        private readonly IUserService _userService;
        private readonly IVacationPlanService _vacationPlanService;
        private readonly IJwtTokenOpsServicecs _jwtTokenOpsServicecs;
        private readonly IUserActionsService _userActionsService;
        public VacationPlanController(IUserActionsService userActionsService, IUserService userService,IJwtTokenOpsServicecs jwtTokenOpsServicecs, IVacationPlanService vacationPlanService)
        {
            _userService = userService;
            _jwtTokenOpsServicecs = jwtTokenOpsServicecs;
            _vacationPlanService = vacationPlanService;
            _userActionsService = userActionsService;
        }
       
        [HttpPost("add")]
        public async Task<IActionResult> Add(VacationPlanDTO plan)
        {
            try
            {
                if (Request.Cookies.TryGetValue("jwtToken", out string jwtToken))
                {
                    var Id = _jwtTokenOpsServicecs.getUserIdFromToken(jwtToken);

                    if (Id == null)
                    {
                        return StatusCode(StatusCodes.Status401Unauthorized, "User ID claim not found in the token.");
                    }
                    else
                    {
                        var result = await  _userActionsService.AddVacationPlan(plan,Id);

                        var response = new ObjectResult(result)
                        {
                            StatusCode = StatusCodes.Status201Created,
                        };

                        return response;
                    }
                }
                else
                {
                    var result = new AddVacationPlanResultDTO
                    {
                        success = false,
                        Messages = new[]
                        {
                            "User ID claim not found in the token."
                        }
                    };
                    return new ObjectResult(result)
                    {
                        StatusCode = StatusCodes.Status401Unauthorized,
                    };

                }
            }
            catch (Exception ex)
            {
                var result = new AddVacationPlanResultDTO
                {
                    success = false,
                    Messages = new[]
                    {
                            "Internal server error"
                    }
                };
                return new ObjectResult(result)
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
        }
        
        [HttpGet("plans")]
        public async Task<IActionResult> GetUserPlans()
        {
            try
            {
                if (Request.Cookies.TryGetValue("jwtToken", out string jwtToken))
                {
                    var Id = _jwtTokenOpsServicecs.getUserIdFromToken(jwtToken);

                    if (Id == null)
                    {
                        return StatusCode(StatusCodes.Status401Unauthorized, "User ID claim not found in the token.");
                    }
                    else
                    {
                        var plans = await _vacationPlanService.GetUserPlans(Id);

                        var response = new ObjectResult(plans)
                        {
                            StatusCode = StatusCodes.Status200OK,
                        };

                        return response;
                    }
                }
                else
                {
                    return Unauthorized();

                }

            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
