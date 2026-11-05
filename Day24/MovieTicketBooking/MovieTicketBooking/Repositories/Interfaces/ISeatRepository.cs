using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces;

public interface ISeatRepository
{
    // Add one seat
    Task<Seat> AddAsync(Seat seat);

    // Add multiple seats
    Task<List<Seat>> AddRangeAsync(List<Seat> seats);

    // Get all seats belonging to a screen
    Task<List<Seat>> GetByScreenIdAsync(int screenId);

    // Get a seat by ID
    Task<Seat?> GetByIdAsync(int seatId);

    // Update a seat
    Task UpdateAsync(Seat seat);

    // Delete a seat
    Task DeleteAsync(Seat seat);

    // Check whether a screen exists
    Task<bool> ScreenExistsAsync(int screenId);

    // Check whether the seat already exists in the same position
    Task<bool> SeatExistsAsync(
        int screenId,
        string rowName,
        int seatNumber,
        int? excludeSeatId = null);

    // Check whether the seat has show history
    Task<bool> HasShowSeatsAsync(int seatId);

    // Check whether the seat has booking history
    Task<bool> HasBookingSeatsAsync(int seatId);
}