using MovieTicketBooking.DTOs.Bookings;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Services.Interfaces;

public interface IBookingService
{
    // Create a booking
    Task<BookingResponse> CreateBookingAsync(
        CreateBookingRequest request);

    // Get all bookings with optional filters
    Task<List<BookingResponse>> GetAllBookingsAsync(
        int? showId,
        BookingStatus? status);

    // Get booking by ID
    Task<BookingResponse?> GetBookingByIdAsync(int bookingId);

    // Get bookings for a particular show
    Task<List<BookingResponse>> GetBookingsByShowIdAsync(
        int showId);

    // Cancel a booking
    Task<BookingResponse?> CancelBookingAsync(int bookingId);
}