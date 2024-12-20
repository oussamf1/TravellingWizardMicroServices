﻿using PathComputationMicroService.DTOs;

namespace PathComputationMicroService.Models
{
    public class VacationPlan
    {
        public DateTime Vacation_start_date { get; }
        public DateTime Vacation_end_date { get; }
        public string[] Cities_to_visit { get; }

        public List<LocationStayDuration> CityDaysStayed = new List<LocationStayDuration>();
       
        public Location Starting_Location { get; }

        public Location Ending_Location { get; }

        public VacationPlan(DateTime vacation_start_date, DateTime vacation_end_date, List<LocationStayDuration> cityDaysStayed, Location starting_Location, Location endingLocation)
        {
            Vacation_start_date = vacation_start_date;
            Vacation_end_date = vacation_end_date;
            CityDaysStayed = cityDaysStayed;
            Starting_Location = starting_Location;
            Ending_Location = endingLocation;
        }
    }
}
