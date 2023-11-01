using Shared.Models;

namespace Shared.Services.Interface
{
    public interface IVacationPlanService
    {
        Task<IEnumerable<VacationPlan>> GetUserPlans(int userId);
        Task<IEnumerable<VacationPlan>> GetNonGeneratedPlans();
        Task<bool> UpdatePlan(VacationPlan plan);

    }
}
