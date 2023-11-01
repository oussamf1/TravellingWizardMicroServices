namespace UserOperationsMicroService.DTOs
{
    public class AddVacationPlanResultDTO
    {
        public bool success { get; set; }
        public IEnumerable<string> Messages { get; set; }
    }
}
