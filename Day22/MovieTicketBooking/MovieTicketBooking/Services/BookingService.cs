using MovieTicketBooking.DTOs.Bookings;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;

    public BookingService(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    // Create a booking
    public async Task<BookingResponse> CreateBookingAsync(
        CreateBookingRequest request)
    {
        // Validate seat selection
        if (request.SeatIds == null || request.SeatIds.Count == 0)
        {
            throw new InvalidOperationException(
                "Please select at least one seat.");
        }

        // Check duplicate seat IDs
        if (request.SeatIds.Distinct().Count() != request.SeatIds.Count)
        {
            throw new InvalidOperationException(
                "Duplicate seat IDs are not allowed.");
        }

        // Get show details
        var show = await _bookingRepository.GetShowByIdAsync(
            request.ShowId);

        if (show == null)
        {
            throw new InvalidOperationException(
                "Show not found.");
        }

        // Get selected seats for this show
        var showSeats = await _bookingRepository.GetShowSeatsAsync(
            request.ShowId,
            request.SeatIds);

        // Check whether all requested seats belong to this show
        if (showSeats.Count != request.SeatIds.Count)
        {
            throw new InvalidOperationException(
                "One or more selected seats do not belong to this show.");
        }

        // Check seat availability
        if (showSeats.Any(ss => ss.Status != ShowSeatStatus.Available))
        {
            throw new InvalidOperationException(
                "One or more selected seats are already booked.");
        }

        // Calculate total amount on the server
        decimal totalAmount = show.TicketPrice * request.SeatIds.Count;

        // Create booking entity
        var booking = new Booking
        {
            BookingReference = "TEMP" + Guid.NewGuid().ToString("N")[..10],
            ShowId = request.ShowId,
            CustomerName = request.CustomerName,
            CustomerEmail = request.CustomerEmail,
            CustomerPhone = request.CustomerPhone,
            BookingDate = DateTime.UtcNow,
            TotalAmount = totalAmount,
            Status = BookingStatus.Confirmed
        };

        // Create booking seat records
        var bookingSeats = showSeats.Select(ss => new BookingSeat
        {
            SeatId = ss.SeatId,
            Price = show.TicketPrice
        }).ToList();

        // Save booking and update seat statuses
        var createdBooking =
            await _bookingRepository.CreateBookingAsync(
                booking,
                bookingSeats,
                showSeats);

        return MapToResponse(createdBooking);
    }

    // Get all bookings with optional filters
    public async Task<List<BookingResponse>> GetAllBookingsAsync(
        int? showId,
        BookingStatus? status)
    {
        var bookings = await _bookingRepository.GetAllAsync(
            showId,
            status);

        return bookings.Select(MapToResponse).ToList();
    }

    // Get booking by ID
    public async Task<BookingResponse?> GetBookingByIdAsync(
        int bookingId)
    {
        var booking = await _bookingRepository.GetByIdAsync(
            bookingId);

        if (booking == null)
        {
            return null;
        }

        return MapToResponse(booking);
    }

    // Get bookings for a particular show
    public async Task<List<BookingResponse>> GetBookingsByShowIdAsync(
        int showId)
    {
        var bookings = await _bookingRepository.GetByShowIdAsync(
            showId);

        return bookings.Select(MapToResponse).ToList();
    }

    // Cancel a booking
    public async Task<BookingResponse?> CancelBookingAsync(
        int bookingId)
    {
        var booking = await _bookingRepository.GetByIdAsync(
            bookingId);

        if (booking == null)
        {
            return null;
        }

        // Only confirmed bookings can be cancelled
        if (booking.Status != BookingStatus.Confirmed)
        {
            throw new InvalidOperationException(
                "Only confirmed bookings can be cancelled.");
        }

        // Get the seats belonging to this booking
        var seatIds = booking.BookingSeats
            .Select(bs => bs.SeatId)
            .ToList();

        var showSeats = await _bookingRepository.GetShowSeatsAsync(
            booking.ShowId,
            seatIds);

        if (showSeats.Count != seatIds.Count)
        {
            throw new InvalidOperationException(
                "The booking seat records could not be matched with the show.");
        }

        // Cancel booking and release seats
        await _bookingRepository.CancelBookingAsync(
            booking,
            showSeats);

        return MapToResponse(booking);
    }

    // Convert entity to response DTO
    private static BookingResponse MapToResponse(Booking booking)
    {
        return new BookingResponse
        {
            BookingId = booking.BookingId,
            BookingReference = booking.BookingReference,
            ShowId = booking.ShowId,
            TotalAmount = booking.TotalAmount,
            Status = booking.Status.ToString(),

            Seats = booking.BookingSeats
                .Select(bs =>
                    $"{bs.Seat.RowName}{bs.Seat.SeatNumber}")
                .ToList()
        };
    }
}