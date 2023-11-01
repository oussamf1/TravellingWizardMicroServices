using PathComputationMicroService.DataSources.Interface;
using PathComputationMicroService.Models;

namespace PathComputationMicroService.Services.Interface
{
    public interface IPathComputationService
    {
        public Task<IEnumerable<TripsPlan>> GetTrips(VacationPlan vacationPlan);
 
    }
}
