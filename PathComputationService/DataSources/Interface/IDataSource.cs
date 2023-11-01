using PathComputationMicroService.Models;

namespace PathComputationMicroService.DataSources.Interface
{
    public interface IDataSource
    {
        Task<List<Trip>> GetTrips(VacationPlan vacation_plan);
    }
}
