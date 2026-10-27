using FlightBookingSystem.Models;
using FlightBookingSystem.Repository;
using FlightBookingSystem.Services;

string connectionString =
    @"Server=.\SQLEXPRESS;Database=FlightBookingDB;Trusted_Connection=True;TrustServerCertificate=True;";

FlightBookingService service = new FlightBookingService(connectionString);

FlightBookingRepository flightBookingRepository = new FlightBookingRepository(connectionString);

while (true)
{
    Console.WriteLine("\n--- FLIGHT BOOKING SYSTEM ---");
    Console.WriteLine("1. Add Flight");
    Console.WriteLine("2. View Flights");
    Console.WriteLine("3. Update Flight");
    Console.WriteLine("4. Delete Flight");
    Console.WriteLine("5. Book Flight");
    Console.WriteLine("6. View Bookings");
    Console.WriteLine("7. Exit");
    Console.Write("Enter choice: ");

    if (!int.TryParse(Console.ReadLine(), out int choice))
    {
        Console.WriteLine("Enter a valid number.");
        continue;
    }

    switch (choice)
    {
        case 1:
            Flight newFlight = new Flight();

            Console.Write("Flight Name: ");
            newFlight.FlightName = Console.ReadLine() ?? "";

            Console.Write("Source: ");
            newFlight.Source = Console.ReadLine() ?? "";

            Console.Write("Destination: ");
            newFlight.Destination = Console.ReadLine() ?? "";

            Console.Write("Ticket Price: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price)
                || price <= 0)
            {
                Console.WriteLine("Enter a valid positive price.");
                break;
            }

            if (string.IsNullOrWhiteSpace(newFlight.FlightName) ||
                string.IsNullOrWhiteSpace(newFlight.Source) ||
                string.IsNullOrWhiteSpace(newFlight.Destination))
            {
                Console.WriteLine("Flight details cannot be empty.");
                break;
            }

            newFlight.Price = price;
            await service.AddFlight(newFlight);
            break;

        case 2:
            List<Flight> flights = await service.GetAllFlights();

            if (flights.Count == 0)
            {
                Console.WriteLine("No flights available.");
                break;
            }

            foreach (Flight f in flights)
            {
                Console.WriteLine(
                    $"ID: {f.FlightId} | {f.FlightName} | " +
                    $"{f.Source} -> {f.Destination} | Price: {f.Price}");
            }
            break;

        case 3:
            Console.Write("Enter Flight ID: ");
            if (!int.TryParse(Console.ReadLine(), out int updateId))
            {
                Console.WriteLine("Invalid ID.");
                break;
            }

            if (service.GetFlightById(updateId) == null)
            {
                Console.WriteLine("Flight not found.");
                break;
            }

            Flight updatedFlight = new Flight();

            Console.Write("New Flight Name: ");
            updatedFlight.FlightName = Console.ReadLine() ?? "";

            Console.Write("New Source: ");
            updatedFlight.Source = Console.ReadLine() ?? "";

            Console.Write("New Destination: ");
            updatedFlight.Destination = Console.ReadLine() ?? "";

            Console.Write("New Ticket Price: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal newPrice)
                || newPrice <= 0)
            {
                Console.WriteLine("Invalid price.");
                break;
            }

            if (string.IsNullOrWhiteSpace(updatedFlight.FlightName) ||
                string.IsNullOrWhiteSpace(updatedFlight.Source) ||
                string.IsNullOrWhiteSpace(updatedFlight.Destination))
            {
                Console.WriteLine("Flight details cannot be empty.");
                break;
            }

            updatedFlight.Price = newPrice;

            Console.WriteLine(
               await service.UpdateFlight(updateId, updatedFlight)
                    ? "Flight updated successfully."
                    : "Flight not found.");
            break;

        case 4:
            Console.Write("Enter Flight ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int deleteId))
            {
                Console.WriteLine("Invalid ID.");
                break;
            }

            Console.WriteLine(
              await  service.DeleteFlight(deleteId)
                    ? "Flight deleted successfully."
                    : "Flight not found.");
            break;

        case 5:
            Console.Write("Enter Flight ID: ");
            if (!int.TryParse(Console.ReadLine(), out int flightId))
            {
                Console.WriteLine("Invalid ID.");
                break;
            }

            Console.Write("Passenger Name: ");
            string passenger = Console.ReadLine() ?? "";

            Console.Write("Number of Seats: ");
            if (!int.TryParse(Console.ReadLine(), out int seats)
                || seats <= 0)
            {
                Console.WriteLine("Enter a valid number of seats.");
                break;
            }

            Booking? booking = await service.BookFlight(
                flightId, passenger, seats);

            if (booking == null)
            {
                Console.WriteLine("Invalid flight or passenger details.");
                break;
            }

            Console.WriteLine("Booking successful!");
            Console.WriteLine($"Booking ID: {booking.BookingId}");
            Console.WriteLine($"Total Price: {booking.TotalPrice}");
            break;

        case 6:
            List<Booking> bookings =await service.GetAllBookings();

            if (bookings.Count == 0)
            {
                Console.WriteLine("No bookings found.");
                break;
            }

            foreach (Booking b in bookings)
            {
                Console.WriteLine(
                    $"Booking ID: {b.BookingId} | " +
                    $"Passenger: {b.PassengerName} | " +
                    $"Flight ID: {b.FlightId} | " +
                    $"Seats: {b.Seats} | Total: {b.TotalPrice}");
            }
            break;

        case 7:
            Console.WriteLine("Thank you!");
            return;

        default:
            Console.WriteLine("Invalid choice.");
            break;
    }
}