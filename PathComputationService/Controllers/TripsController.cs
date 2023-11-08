using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PathComputationMicroService.DTOs;
using PathComputationMicroService.Models;
using PathComputationMicroService.Services.Concrete;
using PathComputationMicroService.Services.Interface;

namespace PathComputationMicroService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TripsController : Controller
    {
        private readonly IPathComputationService _pathComputationService;
        public TripsController(IPathComputationService pathComputationService)
        {
            _pathComputationService = pathComputationService;
        }
        [HttpPost("getPlansForUser")]
        public async Task<IEnumerable<TripsPlan>> GetTripsForUser(VacationPlanDTO plan)
        {
            return await Get(plan);
        }
        [HttpPost("getPlansForService")]
        [Authorize(AuthenticationSchemes = "ApiKeyAuthentication")]
        public async Task<IEnumerable<TripsPlan>> GetTripsForService(VacationPlanDTO plan)
        {
            return await Get(plan);
        }
        private async Task<IEnumerable<TripsPlan>> Get(VacationPlanDTO plan)
        {
            DateTime start =  DateTime.Parse(plan.VacationStartDate);
            DateTime end = DateTime.Parse(plan.VacationEndDate);
            Location starting_location = new Location(plan.StartingLocation.City, plan.StartingLocation.IATA,plan.StartingLocation.Country);
            Location ending_location = new Location(plan.EndingLocation.City, plan.EndingLocation.IATA, plan.EndingLocation.Country);
            VacationPlan myVacationPlan = new VacationPlan(start, end, plan.CityDaysStayed, starting_location, ending_location);
            return await _pathComputationService.GetTrips(myVacationPlan);
        }
    }
}
