using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shared.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class Class1 : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Bla { get; set; }
    }
}
