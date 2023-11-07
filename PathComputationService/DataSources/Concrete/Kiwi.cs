using Newtonsoft.Json;
using PathComputationMicroService.DataSources.Interface;
using PathComputationMicroService.DataSources.Models.Kiwi;
using PathComputationMicroService.Models;

namespace PathComputationMicroService.DataSources.Concrete
{
    public class Kiwi : IDataSource
    {
      
        public async Task<List<Trip>> GetTrips(VacationPlan vacation_plan)//combine all trips to find best plan
        {
            List<FlightstInfo> flights_infos = await GetFlights(vacation_plan);
            List<Trip> all_trips = new List<Trip>();
            foreach (var flight_info in flights_infos)
            {
                List<Trip> trips = await FormatData(flight_info);
                all_trips.AddRange(trips);
            }
            return all_trips;
        }
        public string FormatDate(DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }
        public string GetURL(string startDate, string endDate, string origin, string destination)
        {
            string apiUrl = $"https://tequila-api.kiwi.com/v2/search?fly_from={origin}&fly_to={destination}&date_from={startDate}&date_to={endDate}";
            return apiUrl;
        }
        public async Task<FlightstInfo> CallSearcchApiKiwi(string apiUrl)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("apikey", "kQBxPTTxfXNHF3PzLjl5u-0F583dfPsx");
            HttpResponseMessage response = await client.GetAsync(apiUrl);
            string responseBody = await response.Content.ReadAsStringAsync();
            FlightstInfo flightsInfo = JsonConvert.DeserializeObject<FlightstInfo>(responseBody);
            return flightsInfo;
        }
        public async Task<List<FlightstInfo>> GetFlights(VacationPlan vacation_plan)
        {
            string[] cities_to_visit = vacation_plan.CityDaysStayed.Select(location => location.City.IATA).ToArray();
            string startDate = FormatDate(vacation_plan.Vacation_start_date);
            string endDate = FormatDate(vacation_plan.Vacation_end_date);
            List<FlightstInfo> flightstInfos = new List<FlightstInfo>();

            foreach (string city in cities_to_visit)
            {
                string origin = vacation_plan.Starting_Location.City_code;
                string destination = city;
                string apiUrl = GetURL(startDate, endDate, origin, destination);
                FlightstInfo flightsinfo = await CallSearcchApiKiwi(apiUrl);
                flightstInfos.Add(flightsinfo);
            }
            foreach (string city in cities_to_visit)
            {
                string origin = city;
                string destination = vacation_plan.Ending_Location.City_code;
                string apiUrl = GetURL(startDate, endDate, origin, destination);
                FlightstInfo flightsinfo = await CallSearcchApiKiwi(apiUrl);
                flightstInfos.Add(flightsinfo);
            }
            for (int i = 0; i < cities_to_visit.Length; i++)
            {
                for (int j = 0; j < cities_to_visit.Length; j++)
                {
                    if (i != j)
                    {
                        string origin = cities_to_visit[i];
                        string destination = cities_to_visit[j];
                        string apiUrl = GetURL(startDate, endDate, origin, destination);
                        FlightstInfo flightsinfo = await CallSearcchApiKiwi(apiUrl);
                        flightstInfos.Add(flightsinfo);
                    }
                }
            }

            return flightstInfos;
        }
        private async Task<List<Trip>> FormatData(FlightstInfo flightsInfo)
        {
            List<Trip> trips = new List<Trip>();
            foreach (Flight flight in flightsInfo.data)
            {
                Location trip_starting_location = new Location(flight.cityFrom, flight.cityCodeFrom, flight.flyFrom);
                Location trip_ending_location = new Location(flight.cityTo, flight.cityCodeTo, flight.flyTo);
                List<Passage> passages = new List<Passage>();
                foreach (var transit in flight.route)
                {
                    Location passage_starting_location = new Location(transit.cityFrom, transit.cityCodeFrom, transit.flyFrom);
                    Location passage_ending_location = new Location(transit.cityTo, transit.cityCodeTo, transit.flyTo);
                    Passage passage = new Passage(passage_starting_location, passage_ending_location, transit.airline, transit.utc_arrival, transit.utc_departure, transit.flight_no);
                    passages.Add(passage);

                }
                Trip trip = new Trip(trip_starting_location, trip_ending_location, true, false, false, passages, flight.duration.total, flight.price, flightsInfo.curreny);
                trips.Add(trip);
            }
            return trips;
        }
    }
 
}
