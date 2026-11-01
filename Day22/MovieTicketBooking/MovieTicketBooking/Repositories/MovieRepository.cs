using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly ApplicationDbContext _context;

    public MovieRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Add a new movie
    public async Task<Movie> AddAsync(Movie movie)
    {
        await _context.Movies.AddAsync(movie);

        await _context.SaveChangesAsync();

        return movie;
    }

    // Get all movies with pagination and optional title filter
    public async Task<List<Movie>> GetAllAsync(
        int pageNumber,
        int pageSize,
        string? title)
    {
        IQueryable<Movie> query = _context.Movies.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(title))
        {
            query = query.Where(m =>
                m.Title.Contains(title));
        }

        return await query
            .OrderBy(m => m.MovieId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    // Get movie by ID
    public async Task<Movie?> GetByIdAsync(int movieId)
    {
        return await _context.Movies
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.MovieId == movieId);
    }

    // Search movies by title
    public async Task<List<Movie>> SearchByTitleAsync(string title)
    {
        return await _context.Movies
            .AsNoTracking()
            .Where(m => m.Title.Contains(title))
            .OrderBy(m => m.Title)
            .ToListAsync();
    }

    // Update movie
    public async Task UpdateAsync(Movie movie)
    {
        _context.Movies.Update(movie);

        await _context.SaveChangesAsync();
    }

    // Delete movie
    public async Task DeleteAsync(Movie movie)
    {
        _context.Movies.Remove(movie);

        await _context.SaveChangesAsync();
    }

    // Check whether the movie has shows
    public async Task<bool> HasShowsAsync(int movieId)
    {
        return await _context.Shows
            .AnyAsync(s => s.MovieId == movieId);
    }
}