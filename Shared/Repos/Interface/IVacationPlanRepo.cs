using Shared.Models;

namespace Shared.Repos.Interface
{
    public interface IVacationPlanRepo : IGenericRepo<VacationPlan>
    {
        Task<IEnumerable<VacationPlan>> GetUserPlans(int userId);
        Task<IEnumerable<VacationPlan>> GetNonGeneratedPlans();

    }
}
