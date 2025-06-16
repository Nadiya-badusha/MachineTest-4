using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyWithMe_Flights.Model
{
    public class City
    {
        public int CityId { get; set; }
        public string CityName { get; set; } = null!;

        // Foreign key
        public int CountryId { get; set; }

        // Navigation properties
        public Country? Country { get; set; }
        public List<Airport> Airports { get; set; } = new();
    }
}
