using PathComputationMicroService.Models;
using System.Collections.Generic;

namespace PathComputationMicroService.GraphComputation
{
    public class VacationPlanGraph
    {
        private Dictionary<string, Dictionary<string, List<Trip>>> graph;

        private int numOfPlans;

            public VacationPlanGraph()
            {
                graph = new Dictionary<string, Dictionary<string, List<Trip>>>();
                numOfPlans = 5;
            }

            public void AddVertex(string vertex)
            {
                if (!graph.ContainsKey(vertex))
                {
                    graph[vertex] = new Dictionary<string, List<Trip>>();
                }
            }

            public void AddEdge(string start, string end, Trip trip)
            {
                AddVertex(start);
                AddVertex(end);

                if (!graph[start].ContainsKey(end))
                {
                    graph[start][end] = new List<Trip>();
                }

                graph[start][end].Add(trip);
            }

            public List<TripsPlan> TSP(VacationPlan vacationPlan)
            {
                var path = new List<string>();
                var visited = new HashSet<string>();
                var cost = decimal.MaxValue;
                var best_route = new List<Trip>();
                var bestPlans = new List<TripsPlan>();
                string start = vacationPlan.Starting_Location.City_code;
                string end = vacationPlan.Ending_Location.City_code;
                visited.Add(start);
                visited.Add(end);

                var vertices = graph.Keys.Except(new[] { start, end });

                foreach (var permutation in GetPermutations(vertices.ToList()))
                {

                    var currentPath = new List<string> { start };

                    currentPath.AddRange(permutation);

                    currentPath.Add(end);

                    var (trips, currentCost) = ComputePathAndCost(currentPath, vacationPlan);

                    TripsPlan plan = new TripsPlan
                    {
                        Trips = trips,
                        Price = cost,
                    };
                 
                    if ( trips.Count-1  == vacationPlan.CityDaysStayed.Count)
                    {
                        path = currentPath;
                        cost = currentCost;
                        best_route = trips;
                        bestPlans.Add(plan);
                    }
                }

            return bestPlans;
            }
            
            private bool IsBetterPlan (TripsPlan plan, List<TripsPlan> bestPlans) // compare the route to the best 5 accumulated
            {
                if (bestPlans.Count < 5)
                {
                   return false;
                }
                else
                {
                   var mostExpensivePlan = bestPlans.OrderByDescending(plan => plan.Price).FirstOrDefault();

                   if(plan.Price < mostExpensivePlan.Price)
                   {
                      return true;
                   }
                   else
                   {
                     return false;

                   }

                }

            }

        private (List<Trip>, decimal) ComputePathAndCost(List<string> path, VacationPlan vacationPlan)
        {
            decimal cost = 0;
            List<Trip> trips_queue = new List<Trip>();

            bool success = FindPathRecursive(path, vacationPlan, ref cost, trips_queue, 0, new Dictionary<string, List<Trip>>());

            if (success)
            {
                return (trips_queue, cost);
            }
            else
            {
                return (new List<Trip>(), -1); 
            }
        }

        private bool FindPathRecursive(List<string> path, VacationPlan vacationPlan, ref decimal cost, List<Trip> trips_queue, int cityIndex, Dictionary<string, List<Trip>> usedTrips)
        {
            if (cityIndex == path.Count - 1)
            {
                return true;
            }

            string currentCity = path[cityIndex];
            string nextCity = path[cityIndex + 1];
            var edges = graph[currentCity][nextCity];

            edges = edges.Except(usedTrips.ContainsKey(currentCity + nextCity) ? usedTrips[currentCity + nextCity] : Enumerable.Empty<Trip>()).ToList();

            Trip cheapestTrip = null;

            foreach (var trip in edges)
            {
                if (IsValidTrip(trip, trips_queue, vacationPlan))
                {
                    if (cheapestTrip == null || trip.Price < cheapestTrip.Price)
                    {
                        cheapestTrip = trip;
                    }
                }
            }

            if (cheapestTrip != null)
            {
                cost += cheapestTrip.Price;
                trips_queue.Add(cheapestTrip);

                if (!usedTrips.ContainsKey(currentCity + nextCity))
                {
                    usedTrips[currentCity + nextCity] = new List<Trip>();
                }
                usedTrips[currentCity + nextCity].Add(cheapestTrip);

                return FindPathRecursive(path, vacationPlan, ref cost, trips_queue, cityIndex + 1, usedTrips);
            }
            else
            {//Backtrack
                if (trips_queue.Count > 0)
                {
                    var lastTrip = trips_queue.Last();
                    trips_queue.Remove(lastTrip);
                    cost -= lastTrip.Price;

                    if (usedTrips.ContainsKey(currentCity + nextCity))
                    {
                        usedTrips[currentCity + nextCity].Remove(lastTrip);
                    }
                }

                if (cityIndex > 0)
                {
                    return FindPathRecursive(path, vacationPlan, ref cost, trips_queue, cityIndex - 1, usedTrips);
                }
                else
                {
                    return false;
                }
            }
        }

        private static bool IsValidTrip(Trip trip, List<Trip> trips_queue, VacationPlan vacationPlan)// check if the trip comes in a date later than the previous one
            {
                if (trips_queue.Count == 0)
                {
                    return true;
                }
                else
                {
                Trip lastTrip = trips_queue.Last();
                if (lastTrip == null )
                {
                    return false;
                }
                
                else if (TripHasValidStay(lastTrip, trip, vacationPlan))
                {
                        return true;
                }
                else
                {
                        return false;
                }
                }
            }
            public static bool IsValidForVacationPlan(Trip trip, Dictionary<string, int> remainingCityDaysStayed,VacationPlan vacationPlan) //checks if taking this trip allows for satisfying the vacation plan
            {
                int totalRemainingDays = remainingCityDaysStayed.Values.Sum();

                TimeSpan? timeUntilVacationEnd = vacationPlan.Vacation_end_date - trip.Trip_route.Last().Arrival_utc;

                bool isValid = timeUntilVacationEnd.Value.TotalDays > totalRemainingDays;

            return isValid;
            }
        private static bool TripHasValidStay(Trip lastTrip, Trip nextTrip, VacationPlan vacationPlan)
        {
            int stay_duration = vacationPlan.CityDaysStayed[lastTrip.Trip_ending_location.City_code];
            if ((!lastTrip.Trip_route.Last().Arrival_utc.HasValue || !nextTrip.Trip_route.First().Departure_utc.HasValue ||
                 (lastTrip.Trip_route.Last().Arrival_utc.Value.AddDays(stay_duration).CompareTo(nextTrip.Trip_route.First().Departure_utc.Value) < 0)
                 && (lastTrip.Trip_route.Last().Arrival_utc.Value.AddDays(stay_duration).AddHours(30).CompareTo(nextTrip.Trip_route.First().Departure_utc.Value) > 0)))
            {

                return true;
            }
            else
            {
                return false;
            }

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

