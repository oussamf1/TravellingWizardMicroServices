using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Models;
using Shared.Repos.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Repos.Concrete
{
    public class EmailTemplateRepo : GenericRepo<EmailTemplate, ApplicationDbContext>, IEmailTemplateRepo

    {
        private readonly ApplicationDbContext _dbContext;
        public EmailTemplateRepo(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<EmailTemplate> GetByType(string type)
        {
            var emailTemplate = await _dbContext.EmailTemplates.FirstOrDefaultAsync(template => template.Type == type);
            return emailTemplate;
        }
    }
}
