using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyWithMe_Flights.Model
{
    public class Country
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; } = null!;

        // Navigation property
        public List<City> Cities { get; set; } = new();


    }
}
