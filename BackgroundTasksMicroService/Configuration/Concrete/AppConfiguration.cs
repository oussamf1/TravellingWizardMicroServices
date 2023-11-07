using BackgroundTasksMicroService.Configuration.Interface;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundTasksMicroService.Configuration.Concrete
{
    public class AppConfiguration : IAppConfiguration
    {
        public AppConfiguration(IConfiguration config)
        {
            DatabaseConnectionString = config.GetConnectionString("DatabaseConnectionString");
            FrontEndURl = config.GetSection("URLs")["FrontEndURl"];
            ApiKey = config.GetSection("Keys")["ApiKey"];
            ComputationMicroserviceUrl = config.GetSection("URLs")["ComputationMicroserviceUrl"];
        }
        public static IAppConfiguration GetAppConfiguration(IConfiguration config)
        {
            return new AppConfiguration(config);
        }
        public string ApiKey { get; set; }
        public string DatabaseConnectionString { get; set; }
        public string FrontEndURl { get; set; }
        public string ComputationMicroserviceUrl { get; set; }
    }
}
