using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Configuration;
using FlyWithMe_Flights.Repository;
using FlyWithMe_Flights.Service;
using FlyWithMe_Flights.Model;
namespace FlyWithMe_Flights
{
   

    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CsWin"].ConnectionString;

            IFlightrepo userRepo = new FlightrepoImpl(connectionString);
            IFligtservice userService = new FlightServiceImpl(userRepo);

            try
            {

                //useranme and password input

                while (true)
                {

                    Console.WriteLine("-----------Login FlyWithMe----------");

                    Console.Write("Enter username: ");
                    string username = Console.ReadLine()!;

                    Console.Write("Enter password: ");
                    string password = Console.ReadLine()!;

                    bool isLoggedIn = userService.Login(username, password);

                    if (isLoggedIn)
                    {
                        Console.WriteLine("Access granted. Welcome Admin!");
                        AdminMenu();
                    }
                    else
                    {
                        Console.WriteLine("Access denied.");
                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            } 
        }

        public static void AdminMenu()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CsWin"].ConnectionString;

            IFlightrepo userRepo = new FlightrepoImpl(connectionString);
            IFligtservice _service = new FlightServiceImpl(userRepo);

            bool running = true;

            //admin main menu
            while (running)
            {
                
                Console.WriteLine("\n-- Admin Menu --");
                Console.WriteLine("1. List All Flight details.");
                Console.WriteLine("2. Search By Flight Id.");
                Console.WriteLine("3. Add a Flight details.");
                Console.WriteLine("4. Edit and Update Flight details.");
                Console.WriteLine("5. Logout");
                Console.Write("Enter choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("List all Flight Details");

                        foreach (var flight in _service.GetAllFlights())
                            DisplayFlight(flight);
                        break;
                    case "2":
                        Console.WriteLine("Search By Flight Id");
                        Console.Write("Enter Flight ID: ");
                        int fid = int.Parse(Console.ReadLine()!);
                        var f = _service.GetFlightById(fid);
                        if (f != null) DisplayFlight(f); else Console.WriteLine("Flight not found.");
                        break;

                    case "3":
                        Console.WriteLine("Add Flight Details ");
                        var newFlight = InputFlightDetails();
                        _service.AddFlight(newFlight);
                        Console.WriteLine("Flight added.");
                        break;

                    case "4":
                        Console.WriteLine("Update Flight Details ");
                        Console.Write("Enter Flight ID to update: ");
                        int updateId = int.Parse(Console.ReadLine()!);
                        var flightToUpdate = _service.GetFlightById(updateId);
                        if (flightToUpdate != null)
                        {
                            var updated = InputFlightDetails();
                            updated.FlightId = updateId;
                            _service.UpdateFlight(updated);
                            Console.WriteLine("Flight updated.");
                        }
                        else Console.WriteLine("Flight not found.");
                        break;

                    case "5":
                        Console.WriteLine("Logging out...");
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

            }

        }


        public static Flight InputFlightDetails()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CsWin"].ConnectionString;
            IFlightrepo repo = new FlightrepoImpl(connectionString);

            Console.WriteLine("----- Enter Departure Information -----");
            int depAirportId = SelectAirport(repo);

            Console.WriteLine("----- Enter Arrival Information -----");
            int arrAirportId = SelectAirport(repo);
            
            DateTime depDate = ReadValidDate("Enter Departure Date");

            TimeSpan depTime = ReadValidTime("Enter Departure Time");

            DateTime arrDate = ReadValidDate("Enter Arrival Date");
            TimeSpan arrTime = ReadValidTime("Enter Arrival Time");

            Console.Write("Flight Status: ");
            string status = Console.ReadLine()!;

            return new Flight
            {
                DepAirportId = depAirportId,
                ArrAirportId = arrAirportId,
                DepDate = depDate,
                DepTime = depTime,
                ArrDate = arrDate,
                ArrTime = arrTime,
                FlightStatus = status
            };
        }
        public static int SelectAirport(IFlightrepo repo)
        {
            var countries = repo.GetAllCountries().ToList();
            Console.WriteLine("Select Country:");
            for (int i = 0; i < countries.Count; i++)
                Console.WriteLine($"{i + 1}. {countries[i].CountryName}");

            int countryChoice = int.Parse(Console.ReadLine()!) - 1;
            int countryId = countries[countryChoice].CountryId;

            var cities = repo.GetCitiesByCountry(countryId).ToList();
            Console.WriteLine("Select City:");
            for (int i = 0; i < cities.Count; i++)
                Console.WriteLine($"{i + 1}. {cities[i].CityName}");

            int cityChoice = int.Parse(Console.ReadLine()!) - 1;
            int cityId = cities[cityChoice].CityId;

            var airports = repo.GetAirportsByCity(cityId).ToList();
            Console.WriteLine("Select Airport:");
            for (int i = 0; i < airports.Count; i++)
                Console.WriteLine($"{i + 1}. {airports[i].AirportName} ({airports[i].AirportCode})");

            int airportChoice = int.Parse(Console.ReadLine()!) - 1;
            return airports[airportChoice].AirportId;
        }


        public static void DisplayFlight(Flight f)
        {
            Console.WriteLine("\n---------------------------------------------------------------");
            Console.WriteLine($"Flight ID: {f.FlightId}");
            Console.WriteLine($"From: {f.DepAirportName}, {f.DepCityName}, {f.DepCountryName}");
            Console.WriteLine($"To:   {f.ArrAirportName}, {f.ArrCityName}, {f.ArrCountryName}");
            Console.WriteLine($"Departure: {f.DepDate.ToShortDateString()} {f.DepTime}");
            Console.WriteLine($"Arrival:   {f.ArrDate.ToShortDateString()} {f.ArrTime}");
            Console.WriteLine($"Status: {f.FlightStatus}");
            Console.WriteLine("-------------------------------------------------------------------");
        }

        public static DateTime ReadValidDate(string prompt)
        {
            DateTime date;
            while (true)
            {
                Console.Write($"{prompt} (yyyy-MM-dd): ");
                string? input = Console.ReadLine();

                if (DateTime.TryParseExact(input, "yyyy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out date))
                {
                    break;
                }
                Console.WriteLine("Invalid date format. Please enter in yyyy-MM-dd format.");
            }
            return date;
        }

        public static TimeSpan ReadValidTime(string prompt)
        {
            TimeSpan time;
            while (true)
            {
                Console.Write($"{prompt} (HH:mm): ");
                string? input = Console.ReadLine();

                if (TimeSpan.TryParseExact(input, "hh\\:mm",
                    System.Globalization.CultureInfo.InvariantCulture, out time))
                {
                    break;
                }
                Console.WriteLine("Invalid time format. Please enter in HH:mm (24-hour) format.");
            }
            return time;
        }
    }
}
        
    