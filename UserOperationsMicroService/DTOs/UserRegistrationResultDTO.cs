namespace UserOperationsMicroService.DTOs
{
    public class UserRegistrationResultDTO
    {
        public bool isRegistered { get; set; } 
        public IEnumerable<string> ErrosMessages { get; set; }

    }
}
