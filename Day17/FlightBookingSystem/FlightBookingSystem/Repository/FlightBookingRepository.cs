
using FlightBookingSystem.Models;
using Microsoft.Data.SqlClient;

namespace FlightBookingSystem.Repository
{
    public class FlightBookingRepository
    {
        private readonly string _connectionString;

        public FlightBookingRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddFlightAsync(Flight flight)
        {
            using SqlConnection connection =
                new SqlConnection(_connectionString);

            await connection.OpenAsync();

            string query = @"INSERT INTO Flights
                (FlightName, Source, Destination, Price)
                VALUES
                (@FlightName, @Source, @Destination, @Price)";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@FlightName", flight.FlightName);
            command.Parameters.AddWithValue("@Source", flight.Source);
            command.Parameters.AddWithValue("@Destination", flight.Destination);
            command.Parameters.AddWithValue("@Price", flight.Price);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<Flight>> GetAllFlightsAsync()
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            string query = "SELECT * FROM Flights";
            using SqlCommand command = new SqlCommand(query, connection);

            using SqlDataReader reader = await command.ExecuteReaderAsync();
            List<Flight> flights = new List<Flight>();

            while (await reader.ReadAsync())
            {
                Flight flight = new Flight
                {
                    FlightId = reader.GetInt32(0),
                    FlightName = reader.GetString(1),
                    Source = reader.GetString(2),
                    Destination = reader.GetString(3),
                    Price = reader.GetDecimal(4)
                };
                flights.Add(flight);
            }

            return flights;
        }

        public async Task<Flight?> GetFlightByIdAsync(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString + ";Connect Timeout=60");
            await connection.OpenAsync();
            string query = "SELECT * FROM Flights WHERE FlightId = @FlightId";
            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FlightId", id);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                Flight flight = new Flight
                {
                    FlightId = reader.GetInt32(0),
                    FlightName = reader.GetString(1),
                    Source = reader.GetString(2),
                    Destination = reader.GetString(3),
                    Price = reader.GetDecimal(4)
                };
                return flight;
            }
            return null;
        }   
        public async Task<bool> UpdateFlightAsync(int id, Flight updatedFlight)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            string query = @"UPDATE Flights
                             SET FlightName = @FlightName,
                                 Source = @Source,
                                 Destination = @Destination,
                                 Price = @Price
                             WHERE FlightId = @FlightId";
            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FlightId", id);
            command.Parameters.AddWithValue("@FlightName", updatedFlight.FlightName);
            command.Parameters.AddWithValue("@Source", updatedFlight.Source);
            command.Parameters.AddWithValue("@Destination", updatedFlight.Destination);
            command.Parameters.AddWithValue("@Price", updatedFlight.Price);
            int rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

       public async Task<bool> DeleteFlightAsync(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            string query = "DELETE FROM Flights WHERE FlightId = @FlightId";
            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FlightId", id);
            int rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }


        public async Task<Booking?> AddBookingAsync(Booking booking)
        {
            using SqlConnection connection =
                new SqlConnection(_connectionString);

            await connection.OpenAsync();

            string query = @"
        INSERT INTO Bookings
            (FlightId, PassengerName, Seats, TotalPrice)
        VALUES
            (@FlightId, @PassengerName, @Seats, @TotalPrice);

        SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@FlightId", booking.FlightId);
            command.Parameters.AddWithValue("@PassengerName", booking.PassengerName);
            command.Parameters.AddWithValue("@Seats", booking.Seats);
            command.Parameters.AddWithValue("@TotalPrice", booking.TotalPrice);

            object? result = await command.ExecuteScalarAsync();

            if (result == null || result == DBNull.Value)
            {
                return null;
            }

            booking.BookingId = Convert.ToInt32(result);

            return booking;
        }



        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            using SqlConnection connection =
                new SqlConnection(_connectionString);

            await connection.OpenAsync();

            string query = @"
        SELECT BookingId, FlightId, PassengerName, Seats, TotalPrice
        FROM Bookings";

            using SqlCommand command = new SqlCommand(query, connection);

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            List<Booking> bookings = new List<Booking>();

            while (await reader.ReadAsync())
            {
                Booking booking = new Booking
                {
                    BookingId = reader.GetInt32(0),
                    FlightId = reader.GetInt32(1),
                    PassengerName = reader.GetString(2),
                    Seats = reader.GetInt32(3),
                    TotalPrice = reader.GetDecimal(4)
                };

                bookings.Add(booking);
            }

            return bookings;
        }

    }
}
