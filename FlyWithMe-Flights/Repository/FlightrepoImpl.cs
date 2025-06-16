using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlyWithMe_Flights.Model;
using Microsoft.Data.SqlClient;

namespace FlyWithMe_Flights.Repository
{
    public class FlightrepoImpl:IFlightrepo
    {
        private readonly string _connectionString;

        public FlightrepoImpl(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Users? GetUserByUsername(string username)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var cmd = new SqlCommand("SELECT * FROM Users WHERE Username = @Username", conn);
            cmd.Parameters.AddWithValue("@Username", username);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Users
                {
                    UserId = (int)reader["UserId"],
                    Username = reader["Username"].ToString()!,
                    PasswordHash = reader["PasswordHash"].ToString()!,
                    UserRole = reader["UserRole"].ToString()!.Trim()
                };
            }

            return null;
        }
        public IEnumerable<Flight> GetAllFlights()
        {
            var flights = new List<Flight>();

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var cmd = new SqlCommand(@"
                SELECT f.*, 
                       da.AirportName AS DepAirportName, dc.CityName AS DepCityName, dco.CountryName AS DepCountryName,
                       aa.AirportName AS ArrAirportName, ac.CityName AS ArrCityName, aco.CountryName AS ArrCountryName
                FROM Flights f
                JOIN Airports da ON f.DepAirportId = da.AirportId
                JOIN Cities dc ON da.CityId = dc.CityId
                JOIN Countries dco ON dc.CountryId = dco.CountryId
                JOIN Airports aa ON f.ArrAirportId = aa.AirportId
                JOIN Cities ac ON aa.CityId = ac.CityId
                JOIN Countries aco ON ac.CountryId = aco.CountryId", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                flights.Add(new Flight
                {
                    FlightId = (int)reader["FlightId"],
                    DepAirportId = (int)reader["DepAirportId"],
                    ArrAirportId = (int)reader["ArrAirportId"],
                    DepDate = (DateTime)reader["DepDate"],
                    DepTime = (TimeSpan)reader["DepTime"],
                    ArrDate = (DateTime)reader["ArrDate"],
                    ArrTime = (TimeSpan)reader["ArrTime"],
                    FlightStatus = reader["FlightStatus"].ToString()!,
                    DepAirportName = reader["DepAirportName"].ToString(),
                    DepCityName = reader["DepCityName"].ToString(),
                    DepCountryName = reader["DepCountryName"].ToString(),
                    ArrAirportName = reader["ArrAirportName"].ToString(),
                    ArrCityName = reader["ArrCityName"].ToString(),
                    ArrCountryName = reader["ArrCountryName"].ToString()
                });
            }

            return flights;
        }

        public Flight? GetFlightById(int flightId)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var cmd = new SqlCommand(@"
                SELECT f.*, 
                       da.AirportName AS DepAirportName, dc.CityName AS DepCityName, dco.CountryName AS DepCountryName,
                       aa.AirportName AS ArrAirportName, ac.CityName AS ArrCityName, aco.CountryName AS ArrCountryName
                FROM Flights f
                JOIN Airports da ON f.DepAirportId = da.AirportId
                JOIN Cities dc ON da.CityId = dc.CityId
                JOIN Countries dco ON dc.CountryId = dco.CountryId
                JOIN Airports aa ON f.ArrAirportId = aa.AirportId
                JOIN Cities ac ON aa.CityId = ac.CityId
                JOIN Countries aco ON ac.CountryId = aco.CountryId
                WHERE f.FlightId = @FlightId", conn);

            cmd.Parameters.AddWithValue("@FlightId", flightId);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Flight
                {
                    FlightId = (int)reader["FlightId"],
                    DepAirportId = (int)reader["DepAirportId"],
                    ArrAirportId = (int)reader["ArrAirportId"],
                    DepDate = (DateTime)reader["DepDate"],
                    DepTime = (TimeSpan)reader["DepTime"],
                    ArrDate = (DateTime)reader["ArrDate"],
                    ArrTime = (TimeSpan)reader["ArrTime"],
                    FlightStatus = reader["FlightStatus"].ToString()!,
                    DepAirportName = reader["DepAirportName"].ToString(),
                    DepCityName = reader["DepCityName"].ToString(),
                    DepCountryName = reader["DepCountryName"].ToString(),
                    ArrAirportName = reader["ArrAirportName"].ToString(),
                    ArrCityName = reader["ArrCityName"].ToString(),
                    ArrCountryName = reader["ArrCountryName"].ToString()
                };
            }

            return null;
        }

        public void AddFlight(Flight flight)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var cmd = new SqlCommand(@"
                INSERT INTO Flights (DepAirportId, ArrAirportId, DepDate, DepTime, ArrDate, ArrTime, FlightStatus)
                VALUES (@DepAirportId, @ArrAirportId, @DepDate, @DepTime, @ArrDate, @ArrTime, @FlightStatus)", conn);

            cmd.Parameters.AddWithValue("@DepAirportId", flight.DepAirportId);
            cmd.Parameters.AddWithValue("@ArrAirportId", flight.ArrAirportId);
            cmd.Parameters.AddWithValue("@DepDate", flight.DepDate);
            cmd.Parameters.AddWithValue("@DepTime", flight.DepTime);
            cmd.Parameters.AddWithValue("@ArrDate", flight.ArrDate);
            cmd.Parameters.AddWithValue("@ArrTime", flight.ArrTime);
            cmd.Parameters.AddWithValue("@FlightStatus", flight.FlightStatus);

            cmd.ExecuteNonQuery();
        }

        public void UpdateFlight(Flight flight)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var cmd = new SqlCommand(@"
                UPDATE Flights SET
                DepAirportId = @DepAirportId,
                ArrAirportId = @ArrAirportId,
                DepDate = @DepDate,
                DepTime = @DepTime,
                ArrDate = @ArrDate,
                ArrTime = @ArrTime,
                FlightStatus = @FlightStatus
                WHERE FlightId = @FlightId", conn);

            cmd.Parameters.AddWithValue("@DepAirportId", flight.DepAirportId);
            cmd.Parameters.AddWithValue("@ArrAirportId", flight.ArrAirportId);
            cmd.Parameters.AddWithValue("@DepDate", flight.DepDate);
            cmd.Parameters.AddWithValue("@DepTime", flight.DepTime);
            cmd.Parameters.AddWithValue("@ArrDate", flight.ArrDate);
            cmd.Parameters.AddWithValue("@ArrTime", flight.ArrTime);
            cmd.Parameters.AddWithValue("@FlightStatus", flight.FlightStatus);
            cmd.Parameters.AddWithValue("@FlightId", flight.FlightId);

            cmd.ExecuteNonQuery();
        }
        public IEnumerable<Country> GetAllCountries()
        {
            var countries = new List<Country>();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            var cmd = new SqlCommand("SELECT * FROM Countries", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                countries.Add(new Country
                {
                    CountryId = (int)reader["CountryId"],
                    CountryName = reader["CountryName"].ToString()!
                });
            }
            return countries;
        }

        public IEnumerable<City> GetCitiesByCountry(int countryId)
        {
            var cities = new List<City>();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            var cmd = new SqlCommand("SELECT * FROM Cities WHERE CountryId = @CountryId", conn);
            cmd.Parameters.AddWithValue("@CountryId", countryId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cities.Add(new City
                {
                    CityId = (int)reader["CityId"],
                    CityName = reader["CityName"].ToString()!,
                    CountryId = (int)reader["CountryId"]
                });
            }
            return cities;
        }

        public IEnumerable<Airport> GetAirportsByCity(int cityId)
        {
            var airports = new List<Airport>();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            var cmd = new SqlCommand("SELECT * FROM Airports WHERE CityId = @CityId", conn);
            cmd.Parameters.AddWithValue("@CityId", cityId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                airports.Add(new Airport
                {
                    AirportId = (int)reader["AirportId"],
                    AirportCode = reader["AirportCode"].ToString()!,
                    AirportName = reader["AirportName"].ToString()!,
                    CityId = (int)reader["CityId"]
                });
            }
            return airports;
        }

    }
}
