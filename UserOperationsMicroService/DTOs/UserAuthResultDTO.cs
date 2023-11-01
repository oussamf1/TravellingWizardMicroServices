namespace UserOperationsMicroService.DTOs
{
    public class UserAuthResultDTO
    {
        public bool isAuthenticated { get; set; }
        public string Token { get; set; }
        public IEnumerable<string> ErrorMessages { get; set; } 

    }
}
