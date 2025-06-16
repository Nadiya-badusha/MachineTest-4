using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlyWithMe_Flights.Model;

namespace FlyWithMe_Flights.Service
{
    public interface IFligtservice
    {
        public bool Login(string username, string password);
        public IEnumerable<Flight> GetAllFlights();
        public Flight? GetFlightById(int flightId);
        public void AddFlight(Flight flight);
        public void UpdateFlight(Flight flight);
    }
}
