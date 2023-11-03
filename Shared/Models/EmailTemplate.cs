using Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class EmailTemplate : IEntity
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public string Content { get; set; }
    }
}
