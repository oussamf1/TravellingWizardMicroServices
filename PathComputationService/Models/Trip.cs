namespace PathComputationMicroService.Models
{
    public class Trip
    {
        public Location Trip_starting_location { get; }
        public Location Trip_ending_location { get; }
        public bool Is_flight { get; }
        public bool Is_busRide { get; }
        public bool Is_trainRide { get; }
        public string Trip_id { get; }
        public List<Passage> Trip_route { get; }
        public long Duration { get; }
        public decimal Price { get; }
        public string Currency { get; }

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
