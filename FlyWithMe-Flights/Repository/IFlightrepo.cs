using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlyWithMe_Flights.Model;

namespace FlyWithMe_Flights.Repository
{
    public interface IFlightrepo
    {

       public Users? GetUserByUsername(string username);
        public IEnumerable<Flight> GetAllFlights();
        public Flight? GetFlightById(int flightId);
        public void AddFlight(Flight flight);
        public void UpdateFlight(Flight flight);
        public IEnumerable<Country> GetAllCountries();
        public IEnumerable<City> GetCitiesByCountry(int countryId);
        public IEnumerable<Airport> GetAirportsByCity(int cityId);
    }
}
