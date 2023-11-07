using PathComputationMicroService.Models;

namespace PathComputationMicroService.DTOs
{
    public class VacationPlanDTO
    {   
            public string VacationStartDate { get; set; }
            public string VacationEndDate { get; set; }
            public LocationDTO StartingLocation { get; set; }
            public LocationDTO EndingLocation { get; set; }
            public List<LocationStayDuration> CityDaysStayed { get; set; }
    }
    public class LocationDTO
    {
        public string IATA { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

    }

    public class LocationStayDuration
    {
       public LocationDTO City { get; set; }
       public int stayDuration { get; set; }

    }
}
