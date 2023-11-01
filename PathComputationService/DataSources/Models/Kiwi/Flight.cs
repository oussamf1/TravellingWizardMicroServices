namespace PathComputationMicroService.DataSources.Models.Kiwi
{
    public class Flight
    {
        public string id { get; set; }
        public int? nightsInDest { get; set; }
        public Duration duration { get; set; }
        public string flyFrom { get; set; }
        public string cityFrom { get; set; }
        public string cityCodeFrom { get; set; }
        public Country countryFrom { get; set; }
        public string flyTo { get; set; }
        public string cityTo { get; set; }
        public string cityCodeTo { get; set; }
        public Country countryTo { get; set; }
        public double distance { get; set; }
        public List<string> airlines { get; set; }
        public int pnr_count { get; set; }
        public bool has_airport_change { get; set; }
        public int technical_stops { get; set; }
        public bool throw_away_ticketing { get; set; }
        public bool hidden_city_ticketing { get; set; }
        public decimal price { get; set; }
        public Dictionary<string, decimal> bags_price { get; set; }
        public BagLimit baglimit { get; set; }
        public Availability availability { get; set; }
        public bool facilitated_booking_available { get; set; }
        public Dictionary<string, decimal> conversion { get; set; }
        public decimal quality { get; set; }
        public string booking_token { get; set; }
        public Fare fare { get; set; }
        public PriceDropdown price_dropdown { get; set; }
        public bool virtual_interlining { get; set; }
        public List<Route> route { get; set; }
    }
}
