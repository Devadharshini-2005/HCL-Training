using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories;

public class SeatRepository : ISeatRepository
{
    private readonly ApplicationDbContext _context;

    public SeatRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Add one seat
    public async Task<Seat> AddAsync(Seat seat)
    {
        await _context.Seats.AddAsync(seat);
        await _context.SaveChangesAsync();

        return seat;
    }

    // Add multiple seats
    public async Task<List<Seat>> AddRangeAsync(List<Seat> seats)
    {
        await _context.Seats.AddRangeAsync(seats);
        await _context.SaveChangesAsync();

        return seats;
    }

    // Get all seats belonging to a screen
    public async Task<List<Seat>> GetByScreenIdAsync(int screenId)
    {
        return await _context.Seats
            .AsNoTracking()
            .Where(s => s.ScreenId == screenId)
            .OrderBy(s => s.RowName)
            .ThenBy(s => s.SeatNumber)
            .ToListAsync();
    }

    // Get a seat by ID
    public async Task<Seat?> GetByIdAsync(int seatId)
    {
        return await _context.Seats
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.SeatId == seatId);
    }

    // Update a seat
    public async Task UpdateAsync(Seat seat)
    {
        _context.Seats.Update(seat);
        await _context.SaveChangesAsync();
    }

    // Delete a seat
    public async Task DeleteAsync(Seat seat)
    {
        _context.Seats.Remove(seat);
        await _context.SaveChangesAsync();
    }

    // Check whether a screen exists
    public async Task<bool> ScreenExistsAsync(int screenId)
    {
        return await _context.Screens
            .AnyAsync(s => s.ScreenId == screenId);
    }

    // Check whether the seat position already exists
    public async Task<bool> SeatExistsAsync(
        int screenId,
        string rowName,
        int seatNumber,
        int? excludeSeatId = null)
    {
        var query = _context.Seats.Where(s =>
            s.ScreenId == screenId &&
            s.RowName == rowName &&
            s.SeatNumber == seatNumber);

        if (excludeSeatId.HasValue)
        {
            query = query.Where(s => s.SeatId != excludeSeatId.Value);
        }

        return await query.AnyAsync();
    }

    // Check whether the seat has show history
    public async Task<bool> HasShowSeatsAsync(int seatId)
    {
        return await _context.ShowSeats
            .AnyAsync(ss => ss.SeatId == seatId);
    }

    // Check whether the seat has booking history
    public async Task<bool> HasBookingSeatsAsync(int seatId)
    {
        return await _context.BookingSeats
            .AnyAsync(bs => bs.SeatId == seatId);
    }
}