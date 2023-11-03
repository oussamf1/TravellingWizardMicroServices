using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Shared.Data
{
    public class ApplicationDbContext : DbContext
    {
      
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                 : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<VacationPlan> VacationPlans { get; set; }

        public DbSet<EmailTemplate> EmailTemplates { get; set; }


    }
}
