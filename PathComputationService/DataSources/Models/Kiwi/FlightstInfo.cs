namespace PathComputationMicroService.DataSources.Models.Kiwi
{
    public class FlightstInfo
    {
        public string search_id { get; set; }
        public string curreny { get; set; }
        public List<Flight> data { get; set; }
    }
}
