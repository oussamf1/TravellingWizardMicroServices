using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Shared.Data;

namespace Shared.Models
{
    public class User : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }

        [DefaultValue(false)]
        [Required]
        public bool IsEmailConfirmed { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public DateTime AccountCreationDate { get; set; }

        [Required]
        public string ConfirmationToken { get; set; }
    }
}
