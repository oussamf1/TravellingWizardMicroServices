using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Shared.Configuration.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Configuration.Concrete
{
    public class AppConfiguration : IAppConfiguration
    {
        public AppConfiguration(IConfiguration config)
        {
            DatabaseConnectionString = config.GetConnectionString("DatabaseConnectionString");
            JwtSecret = config.GetSection("JwtConfiguration")["JwtSecret"];
            FrontEndUrl = config.GetSection("URLs")["FrontEndUrl"];
            UserOperationMicroServiceUrl = config.GetSection("URLs")["UserOperationMicroServiceUrl"];
            ApiKey = config.GetSection("Keys")["ApiKey"];

        }
        public string DatabaseConnectionString { get; set; }
        public string JwtSecret { get; set; }
        public string FrontEndUrl { get; set; }
        public string ApiKey { get; set; }

        public string UserOperationMicroServiceUrl { get; set; }
        public static IAppConfiguration GetAppConfiguration(IConfiguration config)
        {
            return new AppConfiguration(config);
        }
    }
}
