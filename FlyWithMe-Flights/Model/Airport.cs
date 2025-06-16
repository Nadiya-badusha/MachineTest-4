using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyWithMe_Flights.Model
{
    public class Airport
    {
        public int AirportId { get; set; }
        public string AirportCode { get; set; } = null!;
        public string AirportName { get; set; } = null!;

        // Foreign key
        public int CityId { get; set; }

        // Navigation properties
        public City? City { get; set; }
        public List<Flight> DepartingFlights { get; set; } = new();
        public List<Flight> ArrivingFlights { get; set; } = new();

    }
}
