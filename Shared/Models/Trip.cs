namespace Shared.Models
{
    public class Trip
    {
        public Location Trip_starting_location { get; set; }
        public Location Trip_ending_location { get; }
        public bool Is_flight { get; set; }
        public bool Is_busRide { get; set; }
        public bool Is_trainRide { get; set; }
        public string Trip_id { get; set; }
        public List<Passage> Trip_route { get; set; }
        public long Duration { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }

        public Trip(Location trip_starting_location, Location trip_ending_location, bool isFlight, bool isBusRide, bool isTrainRide, List<Passage> tripRoute, long duration, decimal price, string currency)
        {
            Trip_starting_location = trip_starting_location;
            Trip_ending_location = trip_ending_location;
            Is_flight = isFlight;
            Is_busRide = isBusRide;
            Is_trainRide = isTrainRide;
            Trip_route = tripRoute;
            Duration = duration;
            Price = price;
            Currency = currency;
        }
    }
}
