using MovieTicketBooking.DTOs.Seats;

namespace MovieTicketBooking.Services.Interfaces;

public interface ISeatService
{
    // Create one seat
    Task<SeatResponse> CreateSeatAsync(CreateSeatRequest request);

    // Create multiple seats
    Task<List<SeatResponse>> CreateSeatsBulkAsync(
        BulkCreateSeatsRequest request);

    // Get all seats belonging to a screen
    Task<List<SeatResponse>> GetSeatsByScreenIdAsync(int screenId);

    // Get a seat by ID
    Task<SeatResponse?> GetSeatByIdAsync(int seatId);

    // Update a seat
    Task<SeatResponse?> UpdateSeatAsync(
        int seatId,
        UpdateSeatRequest request);

    // Delete a seat
    Task<bool> DeleteSeatAsync(int seatId);
}