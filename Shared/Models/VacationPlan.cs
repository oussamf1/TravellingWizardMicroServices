using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Data;

namespace Shared.Models
{
    public class VacationPlan : IEntity
    {
        public int Id { get; set; }
        [ForeignKey("UserId")] 
        public int UserId { get; set; }
        public DateTime SubmissionDate { get; set; }
        [DefaultValue(false)]
        public bool isGenerated { get; set; }
        public string planInJson { get; set; }
        public string? GeneratedPlanInJson { get; set; }

    }
}
