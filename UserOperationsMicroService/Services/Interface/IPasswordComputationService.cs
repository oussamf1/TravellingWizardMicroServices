namespace UserOperationsMicroService.Services.Interface
{
    public interface IPasswordComputationService
    {
        public string ComputePasswordHash(string password);
        public bool VerifyPassword(string password, string storedHashedPassword);

    }
}
