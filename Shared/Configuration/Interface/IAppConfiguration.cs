using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Configuration.Interface
{
    public interface IAppConfiguration
    {
        public string DatabaseConnectionString { get; set; }

        public string JwtSecret { get; set; }

        public string FrontEndUrl { get; set; }
        public string UserOperationMicroServiceUrl { get; set; }
        public string ApiKey { get; set; }
    }
}
