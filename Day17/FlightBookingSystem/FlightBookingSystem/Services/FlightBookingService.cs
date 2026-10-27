using FlightBookingSystem.Models;
using FlightBookingSystem.Repository;

namespace FlightBookingSystem.Services
{
    public class FlightBookingService
    {
        private readonly List<Flight> flights = new();
        private readonly List<Booking> bookings = new();

        //public FlightBookingService() { }
        FlightBookingRepository flightBookingRepository;
        public FlightBookingService(string connectionString) 
        {
            flightBookingRepository =new FlightBookingRepository(connectionString);
        }
       
        private int nextFlightId = 1;
        private int nextBookingId = 1;

        // Add Flight
        public async Task AddFlight(Flight flight)
        {
            flight.FlightId = nextFlightId++;
            //flights.Add(flight);

            await flightBookingRepository.AddFlightAsync(flight);


            Console.WriteLine("Flight added successfully.");
        }

        // View All Flights
        public async Task<List<Flight>> GetAllFlights()
        {
            List<Flight> flights = await flightBookingRepository.GetAllFlightsAsync();
            return flights;
        }

        // Find Flight
        public async Task<Flight?> GetFlightById(int id)
        {
            Flight? flight = await flightBookingRepository.GetFlightByIdAsync(id);
            return flight;
        }

        // Update Flight
        public async Task<bool> UpdateFlight(int id, Flight updatedFlight)
        {
            Flight? flight = await flightBookingRepository.GetFlightByIdAsync(id);
            bool isUpdated = await flightBookingRepository.UpdateFlightAsync(id, updatedFlight);
            return isUpdated;
        }

        // Delete Flight
        public async Task<bool>DeleteFlight(int id)
        {
            Flight? flight = await flightBookingRepository.GetFlightByIdAsync(id);

            bool isDeleted = await flightBookingRepository.DeleteFlightAsync(id);

            return isDeleted;
        }

        // Book Flight

        public async Task<Booking?> BookFlight(
            int flightId, string passengerName, int seats)
        {
            if (flightId <= 0 ||
                seats <= 0 ||
                string.IsNullOrWhiteSpace(passengerName))
            {
                return null;
            }

            Flight? flight = await flightBookingRepository.GetFlightByIdAsync(flightId);

            if (flight == null)
            {
                return null;
            }

            Booking booking = new Booking
            {
                PassengerName = passengerName,
                FlightId = flightId,
                Seats = seats,
                TotalPrice = flight.Price * seats
            };

            Booking? addedBooking = await flightBookingRepository.AddBookingAsync(booking);

            return addedBooking;
        }


        // View All Bookings
        public async Task<List<Booking>> GetAllBookings()
        {
            return await flightBookingRepository.GetAllBookingsAsync();
        }

    }
}