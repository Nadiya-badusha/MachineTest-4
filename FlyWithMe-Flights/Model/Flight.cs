using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyWithMe_Flights.Model
{
    public class Flight
    {
        
            public int FlightId { get; set; }

            // Foreign keys
            public int DepAirportId { get; set; }
            public int ArrAirportId { get; set; }

            // Flight schedule info
            public DateTime DepDate { get; set; }      // Use Date part only
            public TimeSpan DepTime { get; set; }      // Time part

            public DateTime ArrDate { get; set; }
            public TimeSpan ArrTime { get; set; }

            // Navigation properties
            public Airport? DepartureAirport { get; set; }
            public Airport? ArrivalAirport { get; set; }
            public string FlightStatus { get; set; } = "Scheduled";

        public string? DepAirportName { get; set; }
        public string? DepCityName { get; set; }
        public string? DepCountryName { get; set; }

        public string? ArrAirportName { get; set; }
        public string? ArrCityName { get; set; }
        public string? ArrCountryName { get; set; }
    }
}
