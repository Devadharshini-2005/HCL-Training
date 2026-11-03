using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories;

public class ShowRepository : IShowRepository
{
    private readonly ApplicationDbContext _context;

    public ShowRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Add a new show
    public async Task<Show> AddAsync(Show show)
    {
        await _context.Shows.AddAsync(show);
        await _context.SaveChangesAsync();

        return show;
    }

    // Get all shows with optional filters
    public async Task<List<Show>> GetAllAsync(
        DateTime? date,
        int? movieId,
        int? theatreId)
    {
        var query = _context.Shows
            .AsNoTracking()
            .Include(s => s.Movie)
            .Include(s => s.Screen)
                .ThenInclude(screen => screen.Theatre)
            .AsQueryable();

        if (date.HasValue)
        {
            var selectedDate = date.Value.Date;
            var nextDate = selectedDate.AddDays(1);

            query = query.Where(s =>
                s.StartTime >= selectedDate &&
                s.StartTime < nextDate);
        }

        if (movieId.HasValue)
        {
            query = query.Where(s => s.MovieId == movieId.Value);
        }

        if (theatreId.HasValue)
        {
            query = query.Where(s =>
                s.Screen.TheatreId == theatreId.Value);
        }

        return await query
            .OrderBy(s => s.StartTime)
            .ToListAsync();
    }

    // Get show by ID
    public async Task<Show?> GetByIdAsync(int showId)
    {
        return await _context.Shows
            .AsNoTracking()
            .Include(s => s.Movie)
            .Include(s => s.Screen)
                .ThenInclude(screen => screen.Theatre)
            .FirstOrDefaultAsync(s => s.ShowId == showId);
    }

    // Get shows by movie ID
    public async Task<List<Show>> GetByMovieIdAsync(int movieId)
    {
        return await _context.Shows
            .AsNoTracking()
            .Where(s => s.MovieId == movieId)
            .OrderBy(s => s.StartTime)
            .ToListAsync();
    }

    // Get shows by theatre ID
    public async Task<List<Show>> GetByTheatreIdAsync(int theatreId)
    {
        return await _context.Shows
            .AsNoTracking()
            .Where(s => s.Screen.TheatreId == theatreId)
            .OrderBy(s => s.StartTime)
            .ToListAsync();
    }

    // Search shows by date
    public async Task<List<Show>> SearchByDateAsync(DateTime date)
    {
        var selectedDate = date.Date;
        var nextDate = selectedDate.AddDays(1);

        return await _context.Shows
            .AsNoTracking()
            .Where(s =>
                s.StartTime >= selectedDate &&
                s.StartTime < nextDate)
            .OrderBy(s => s.StartTime)
            .ToListAsync();
    }

    // Update show
    public async Task UpdateAsync(Show show)
    {
        _context.Shows.Update(show);
        await _context.SaveChangesAsync();
    }

    // Delete show
    public async Task DeleteAsync(Show show)
    {
        _context.Shows.Remove(show);
        await _context.SaveChangesAsync();
    }

    // Check whether movie exists
    public async Task<bool> MovieExistsAsync(int movieId)
    {
        return await _context.Movies
            .AnyAsync(m => m.MovieId == movieId);
    }

    // Check whether screen exists
    public async Task<bool> ScreenExistsAsync(int screenId)
    {
        return await _context.Screens
            .AnyAsync(s => s.ScreenId == screenId);
    }

    // Check whether show has bookings
    public async Task<bool> HasBookingsAsync(int showId)
    {
        return await _context.Bookings
            .AnyAsync(b => b.ShowId == showId);
    }

    // Check overlapping shows on the same screen
    public async Task<bool> HasOverlappingShowAsync(
        int screenId,
        DateTime startTime,
        DateTime endTime,
        int? excludeShowId = null)
    {
        var query = _context.Shows.Where(s =>
            s.ScreenId == screenId &&
            startTime < s.EndTime &&
            endTime > s.StartTime);

        if (excludeShowId.HasValue)
        {
            query = query.Where(s =>
                s.ShowId != excludeShowId.Value);
        }

        return await query.AnyAsync();
    }

    // Get all physical seats belonging to a screen
    public async Task<List<Seat>> GetSeatsByScreenIdAsync(int screenId)
    {
        return await _context.Seats
            .Where(s => s.ScreenId == screenId)
            .ToListAsync();
    }

    // Create ShowSeats records for a show
    public async Task AddShowSeatsAsync(List<ShowSeat> showSeats)
    {
        await _context.ShowSeats.AddRangeAsync(showSeats);
        await _context.SaveChangesAsync();
    }

    // Get all seats for a show
    public async Task<List<ShowSeat>> GetShowSeatsAsync(int showId)
    {
        return await _context.ShowSeats
            .AsNoTracking()
            .Include(ss => ss.Seat)
            .Where(ss => ss.ShowId == showId)
            .OrderBy(ss => ss.Seat.RowName)
            .ThenBy(ss => ss.Seat.SeatNumber)
            .ToListAsync();
    }

    // Get seats by status for a show
    public async Task<List<ShowSeat>> GetShowSeatsByStatusAsync(
        int showId,
        ShowSeatStatus status)
    {
        return await _context.ShowSeats
            .AsNoTracking()
            .Include(ss => ss.Seat)
            .Where(ss =>
                ss.ShowId == showId &&
                ss.Status == status)
            .OrderBy(ss => ss.Seat.RowName)
            .ThenBy(ss => ss.Seat.SeatNumber)
            .ToListAsync();
    }

    // Check whether show exists
    public async Task<bool> ShowExistsAsync(int showId)
    {
        return await _context.Shows
            .AnyAsync(s => s.ShowId == showId);
    }
    // Create a show and its ShowSeats in one transaction
    public async Task<Show> AddShowWithSeatsAsync(
        Show show,
        List<Seat> seats)
    {
        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            // Save the show first to generate ShowId
            await _context.Shows.AddAsync(show);
            await _context.SaveChangesAsync();

            // Generate ShowSeats for all physical seats
            var showSeats = seats.Select(seat => new ShowSeat
            {
                ShowId = show.ShowId,
                SeatId = seat.SeatId,
                Status = ShowSeatStatus.Available
            }).ToList();

            await _context.ShowSeats.AddRangeAsync(showSeats);
            await _context.SaveChangesAsync();

            // Commit both operations
            await transaction.CommitAsync();

            return show;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

}