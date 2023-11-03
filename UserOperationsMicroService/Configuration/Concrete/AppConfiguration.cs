using UserOperationsMicroService.Configuration.Interface;

namespace UserOperationsMicroService.Configuration.Concrete
{
    public class AppConfiguration : IAppConfiguration
    {
        public AppConfiguration(IConfiguration config)
        {
            DatabaseConnectionString = config.GetConnectionString("DatabaseConnectionString");
            JwtSecret = config.GetSection("JwtConfiguration")["JwtSecret"];
            FrontEndUrl = config.GetSection("URLs")["FrontEndUrl"];
            UserOperationMicroServiceUrl = config.GetSection("URLs")["UserOperationMicroServiceUrl"];
        }
        public string DatabaseConnectionString { get; set; }
        public string JwtSecret { get; set; }
        public string FrontEndUrl { get; set; }

        public string UserOperationMicroServiceUrl { get; set; }
        public static IAppConfiguration GetAppConfiguration(IConfiguration config) {
            return new AppConfiguration(config);
        }
    }
}
