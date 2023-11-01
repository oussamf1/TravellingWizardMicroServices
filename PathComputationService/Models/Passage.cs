namespace PathComputationMicroService.Models
{
    public class Passage
    {
        public Location Starting_location { get; }
        public Location Ending_location { get; }
        public string Carrier { get; }
        public string Passage_id { get; }
        public DateTime? Departure_utc { get; set; }
        public DateTime? Arrival_utc { get; set; }


        public Passage(Location startingLocation, Location endingLocation, string carrier, DateTime arrivalUtc, DateTime departureUtc, int passage_id)
        {

            Starting_location = startingLocation;
            Ending_location = endingLocation;
            Carrier = carrier;
            Arrival_utc = arrivalUtc;
            Departure_utc = departureUtc;
            Passage_id = Carrier + passage_id.ToString();
        }
    }
}
