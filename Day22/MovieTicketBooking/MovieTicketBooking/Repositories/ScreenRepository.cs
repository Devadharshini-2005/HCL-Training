using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories;

public class ScreenRepository : IScreenRepository
{
    private readonly ApplicationDbContext _context;

    public ScreenRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Add Screen
    public async Task<Screen> AddAsync(Screen screen)
    {
        await _context.Screens.AddAsync(screen);
        await _context.SaveChangesAsync();

        return screen;
    }

    // Get All Screens with optional Theatre filter
    public async Task<List<Screen>> GetAllAsync(int? theatreId)
    {
        var query = _context.Screens.AsNoTracking();

        if (theatreId.HasValue)
        {
            query = query.Where(s => s.TheatreId == theatreId.Value);
        }

        return await query
            .OrderBy(s => s.ScreenId)
            .ToListAsync();
    }

    // Get Screen By ID
    public async Task<Screen?> GetByIdAsync(int screenId)
    {
        return await _context.Screens
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.ScreenId == screenId);
    }

    // Get Screens By Theatre ID
    public async Task<List<Screen>> GetByTheatreIdAsync(int theatreId)
    {
        return await _context.Screens
            .AsNoTracking()
            .Where(s => s.TheatreId == theatreId)
            .OrderBy(s => s.ScreenId)
            .ToListAsync();
    }

    // Update Screen
    public async Task UpdateAsync(Screen screen)
    {
        _context.Screens.Update(screen);
        await _context.SaveChangesAsync();
    }

    // Delete Screen
    public async Task DeleteAsync(Screen screen)
    {
        _context.Screens.Remove(screen);
        await _context.SaveChangesAsync();
    }

    // Check whether the screen has seats
    public async Task<bool> HasSeatsAsync(int screenId)
    {
        return await _context.Seats
            .AnyAsync(s => s.ScreenId == screenId);
    }

    // Check whether the screen has shows
    public async Task<bool> HasShowsAsync(int screenId)
    {
        return await _context.Shows
            .AnyAsync(s => s.ScreenId == screenId);
    }

    // Check whether the theatre exists
    public async Task<bool> TheatreExistsAsync(int theatreId)
    {
        return await _context.Theatres
            .AnyAsync(t => t.TheatreId == theatreId);
    }
}