using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Shared.Utils;

namespace Shared.Models
{
    public class Passage
    {
        public Location Starting_location { get; set; }
        public Location Ending_location { get; set; }
        public string Carrier { get; set; }
        [JsonProperty("passage_id")]
        public string Passage_id { get; set; }
        public DateTime? Departure_utc { get; set; }
        public DateTime? Arrival_utc { get; set; }


        public Passage(Location startingLocation, Location endingLocation, string carrier, DateTime arrivalUtc, DateTime departureUtc, string passage_id)
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
