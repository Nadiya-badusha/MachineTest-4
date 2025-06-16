using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlyWithMe_Flights.Model;
using FlyWithMe_Flights.Repository;

namespace FlyWithMe_Flights.Service
{

    public class FlightServiceImpl : IFligtservice
    {
        private readonly IFlightrepo _userRepository;

        public FlightServiceImpl(IFlightrepo userRepository)
        {
            _userRepository = userRepository;
        }

        //public FlightServiceImpl()
        //{
        //}

        public bool Login(string username, string password)
        {
            var user = _userRepository.GetUserByUsername(username);
            if (user == null)
            {
                Console.WriteLine("User not found.");
                return false;
            }

            if (user.UserRole != "Administrator")
            {
                Console.WriteLine("Access denied.");
                return false;
            }

            if (user.PasswordHash == password)
            {
                Console.WriteLine("Login successful.");
                return true;
            }

            Console.WriteLine("invalid password.");
            return false;
        }


        public IEnumerable<Flight> GetAllFlights() => _userRepository.GetAllFlights();
        public Flight? GetFlightById(int id) => _userRepository.GetFlightById(id);
        public void AddFlight(Flight flight) => _userRepository.AddFlight(flight);
        public void UpdateFlight(Flight flight) => _userRepository.UpdateFlight(flight);
    }
}
