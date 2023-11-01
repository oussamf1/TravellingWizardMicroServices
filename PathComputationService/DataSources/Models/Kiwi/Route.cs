namespace PathComputationMicroService.DataSources.Models.Kiwi
{
    public class Route
    {
        public string fare_basis { get; set; }
        public string fare_category { get; set; }
        public string fare_classes { get; set; }
        public string fare_family { get; set; }
        public int @return { get; set; }
        public bool bags_recheck_required { get; set; }
        public bool vi_connection { get; set; }
        public bool guarantee { get; set; }
        public string id { get; set; }
        public string combination_id { get; set; }
        public string cityTo { get; set; }
        public string cityFrom { get; set; }
        public string cityCodeFrom { get; set; }
        public string cityCodeTo { get; set; }
        public string flyTo { get; set; }
        public string flyFrom { get; set; }
        public string airline { get; set; }
        public string operating_carrier { get; set; }
        public string equipment { get; set; }
        public int flight_no { get; set; }
        public string vehicle_type { get; set; }
        public string operating_flight_no { get; set; }
        public DateTime local_arrival { get; set; }
        public DateTime utc_arrival { get; set; }
        public DateTime local_departure { get; set; }
        public DateTime utc_departure { get; set; }
    }
}
