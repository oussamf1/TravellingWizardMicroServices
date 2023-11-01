using System.Text.Json;
using Shared.Models;
using Shared.Repos.Interface;
using Shared.Services.Interface;

namespace Shared.Services.Concrete
{
    public class VacationPlanService : IVacationPlanService
    {
        private readonly IVacationPlanRepo _vacationPlanRepo;

        public VacationPlanService(IVacationPlanRepo vacationPlanRepo)
        {
            _vacationPlanRepo = vacationPlanRepo;
        }

        public async Task<IEnumerable<VacationPlan>> GetUserPlans(int userId)
        {
            return await _vacationPlanRepo.GetUserPlans(userId);
        }
        public async Task<IEnumerable<VacationPlan>> GetNonGeneratedPlans()
        {
            return await _vacationPlanRepo.GetNonGeneratedPlans();
        }

        public async Task<bool> UpdatePlan(VacationPlan plan)
        {
            var result = await _vacationPlanRepo.Update(plan);
            return result != null;
        }
    }
}
