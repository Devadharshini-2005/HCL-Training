using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly ApplicationDbContext _context;

    public BookingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Create booking, booking seats and update show seat statuses
    public async Task<Booking> CreateBookingAsync(
        Booking booking,
        List<BookingSeat> bookingSeats,
        List<ShowSeat> showSeats)
    {
        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            // Save booking first to generate BookingId
            booking.BookingReference =
                "TMP" + Guid.NewGuid().ToString("N")[..10];

            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();

            // Generate final booking reference
            booking.BookingReference = $"MOV{booking.BookingId}";

            // Assign the generated BookingId to booking seats
            foreach (var bookingSeat in bookingSeats)
            {
                bookingSeat.BookingId = booking.BookingId;
            }

            await _context.BookingSeats.AddRangeAsync(bookingSeats);

            // Mark selected show seats as Booked
            foreach (var showSeat in showSeats)
            {
                showSeat.Status = ShowSeatStatus.Booked;
            }

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return booking;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    // Get all bookings with optional filters
    public async Task<List<Booking>> GetAllAsync(
        int? showId,
        BookingStatus? status)
    {
        IQueryable<Booking> query = _context.Bookings
            .Include(b => b.BookingSeats)
                .ThenInclude(bs => bs.Seat);

        if (showId.HasValue)
        {
            query = query.Where(b => b.ShowId == showId.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(b => b.Status == status.Value);
        }

        return await query
            .AsNoTracking()
            .OrderByDescending(b => b.BookingDate)
            .ToListAsync();
    }

    // Get booking by ID
    public async Task<Booking?> GetByIdAsync(int bookingId)
    {
        return await _context.Bookings
            .Include(b => b.BookingSeats)
                .ThenInclude(bs => bs.Seat)
            .FirstOrDefaultAsync(b => b.BookingId == bookingId);
    }

    // Get bookings for a particular show
    public async Task<List<Booking>> GetByShowIdAsync(int showId)
    {
        return await _context.Bookings
            .Include(b => b.BookingSeats)
                .ThenInclude(bs => bs.Seat)
            .Where(b => b.ShowId == showId)
            .AsNoTracking()
            .OrderByDescending(b => b.BookingDate)
            .ToListAsync();
    }

    // Get show details
    public async Task<Show?> GetShowByIdAsync(int showId)
    {
        return await _context.Shows
            .FirstOrDefaultAsync(s => s.ShowId == showId);
    }

    // Get requested show seats
    public async Task<List<ShowSeat>> GetShowSeatsAsync(
        int showId,
        List<int> seatIds)
    {
        return await _context.ShowSeats
            .Include(ss => ss.Seat)
            .Where(ss =>
                ss.ShowId == showId &&
                seatIds.Contains(ss.SeatId))
            .ToListAsync();
    }

    // Cancel booking and release seats in a transaction
    public async Task CancelBookingAsync(
        Booking booking,
        List<ShowSeat> showSeats)
    {
        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            booking.Status = BookingStatus.Cancelled;

            foreach (var showSeat in showSeats)
            {
                showSeat.Status = ShowSeatStatus.Available;
            }

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<Booking?> GetByIdAndUserIdAsync(
    int bookingId,
    int userId)
    {
        return await _context.Bookings
            .Include(b => b.BookingSeats)
                .ThenInclude(bs => bs.Seat)
            .FirstOrDefaultAsync(b =>
                b.BookingId == bookingId &&
                b.UserId == userId);
    }

}