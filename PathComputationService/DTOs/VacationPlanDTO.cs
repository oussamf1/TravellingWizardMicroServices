using PathComputationMicroService.Models;

namespace PathComputationMicroService.DTOs
{
    public class VacationPlanDTO
    {   
            public string VacationStartDate { get; set; }
            public string VacationEndDate { get; set; }
            public LocationDTO StartingLocation { get; set; }
            public LocationDTO EndingLocation { get; set; }
            public Dictionary<string, int> CityDaysStayed { get; set; }
    }
    public class LocationDTO
    {
        public string IATA { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

    }
}
