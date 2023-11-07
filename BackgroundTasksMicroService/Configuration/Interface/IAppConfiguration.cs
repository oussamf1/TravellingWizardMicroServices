using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundTasksMicroService.Configuration.Interface
{
    public interface IAppConfiguration
    {
        public string ApiKey { get; set; }

        public string DatabaseConnectionString { get; set; }

        public string FrontEndURl { get; set; }

        public string ComputationMicroserviceUrl { get; set; }
    }
}
