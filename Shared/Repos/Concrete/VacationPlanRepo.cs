using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Models;
using Shared.Repos.Interface;

namespace Shared.Repos.Concrete
{
    public class VacationPlanRepo : GenericRepo<VacationPlan, ApplicationDbContext>, IVacationPlanRepo
    {
        private readonly ApplicationDbContext _dbContext;
        public VacationPlanRepo(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<VacationPlan>> GetUserPlans(int userId)
        {
            return await _dbContext.VacationPlans.Where(plan => plan.UserId == userId).ToListAsync();
        }
        public async Task<IEnumerable<VacationPlan>> GetNonGeneratedPlans()
        {
           return await _dbContext.VacationPlans.Where(plan => plan.isGenerated == false).ToListAsync();
        }
    }
}
