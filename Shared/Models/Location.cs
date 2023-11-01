namespace Shared.Models
{
    public class Location
    {
        public string City { get; set; }
        public string City_code { get; set; }
        public string Terminal { get; set; }
        public Location(string city, string cityCode, string terminal)
        {
            City = city;
            City_code = cityCode;
            Terminal = terminal;
        }

    }
}
