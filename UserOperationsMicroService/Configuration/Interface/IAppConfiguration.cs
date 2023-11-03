namespace UserOperationsMicroService.Configuration.Interface
{
    public interface IAppConfiguration
    {
        public string DatabaseConnectionString { get; set; }

        public string JwtSecret { get; set; }

        public string FrontEndUrl { get; set; }
        public string UserOperationMicroServiceUrl { get; set; }
    }
}
