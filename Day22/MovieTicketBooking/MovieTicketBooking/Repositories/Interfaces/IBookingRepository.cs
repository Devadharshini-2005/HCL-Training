using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces;

public interface IBookingRepository
{
    // Create booking, booking seats and update show seat statuses
    Task<Booking> CreateBookingAsync(
        Booking booking,
        List<BookingSeat> bookingSeats,
        List<ShowSeat> showSeats);

    // Get all bookings with optional filters
    Task<List<Booking>> GetAllAsync(
        int? showId,
        BookingStatus? status);

    // Get booking by ID
    Task<Booking?> GetByIdAsync(int bookingId);

    // Get bookings for a particular show
    Task<List<Booking>> GetByShowIdAsync(int showId);

    // Get show details
    Task<Show?> GetShowByIdAsync(int showId);

    // Get requested show seats
    Task<List<ShowSeat>> GetShowSeatsAsync(
        int showId,
        List<int> seatIds);

    // Cancel booking and release seats in a transaction
    Task CancelBookingAsync(
        Booking booking,
        List<ShowSeat> showSeats);

    Task<Booking?> GetByIdAndUserIdAsync(int bookingId, int userId);

    
}