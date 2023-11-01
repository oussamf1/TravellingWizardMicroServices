using Microsoft.Extensions.Hosting;
using PathComputationMicroService.DataSources.Concrete;
using PathComputationMicroService.DataSources.Interface;
using PathComputationMicroService.GraphComputation;
using PathComputationMicroService.Models;
using PathComputationMicroService.Services.Interface;
using System.IO;

namespace PathComputationMicroService.Services.Concrete
{
    public class PathComputationService : IPathComputationService
    {
        private readonly List<IDataSource> dataSources;
        private VacationPlanGraph vacation_plan_graph;

        public PathComputationService() 
        {

            dataSources = new List<IDataSource>
            {
                new Kiwi()
            };
            vacation_plan_graph = new VacationPlanGraph();
        } async
     
        public Task<IEnumerable<TripsPlan>> GetTrips(VacationPlan vacationPlan)
        {
            string[] cities_to_visit = vacationPlan.CityDaysStayed.Keys.ToArray();
            List<Trip> trips = await Combine_Trips(vacationPlan);
            vacation_plan_graph.AddVertex(vacationPlan.Starting_Location.City_code);
            vacation_plan_graph.AddVertex(vacationPlan.Ending_Location.City_code);
            foreach (var city_to_visit in cities_to_visit)
            {
                vacation_plan_graph.AddVertex(city_to_visit);

            }
            foreach (var trip in trips)
            {
                vacation_plan_graph.AddEdge(trip.Trip_starting_location.City_code, trip.Trip_ending_location.City_code, trip);
            }
            var bestPlans = vacation_plan_graph.TSP(vacationPlan);
            return bestPlans;
        }
    
        private async Task<List<Trip>> Combine_Trips(VacationPlan vacationPlan) // Combine Trips coming from different data Sources into a single list
        {
            List<Trip> trips = new List<Trip>();
            foreach (IDataSource source in dataSources)
            {
                var flights = await source.GetTrips(vacationPlan);
                trips.AddRange(flights);
            }
            return trips;
        }
        private IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list)
        {
            if (list.Count() == 1)
            {
                yield return list;
            }
            else
            {
                foreach (var item in list)
                {
                    var remainingList = list.Where(i => !i.Equals(item));

                    foreach (var permutation in GetPermutations(remainingList))
                    {
                        yield return new[] { item }.Concat(permutation);
                    }
                }
            }
        }

    }
}
