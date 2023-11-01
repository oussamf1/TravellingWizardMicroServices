namespace UserOperationsMicroService.DTOs
{
    public class VacationPlanDTO
    {
        public string VacationStartDate{ get; set; }
        public string VacationEndDate { get; set; }
        public Location StartingLocation { get; set; }
        public Location EndingLocation { get; set; }
        public Dictionary<string, int> CityDaysStayed  { get; set; }

    }
    public class Location
    {
        public string IATA { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

    }
}
