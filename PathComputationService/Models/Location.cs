namespace PathComputationMicroService.Models
{
    public class Location
    {
        public string City { get; }
        public string City_code { get; }
        public string Terminal { get; }

        public Location(string city, string cityCode, string terminal)
        {
            City = city;
            City_code = cityCode;
            Terminal = terminal;
        }
    }
}
