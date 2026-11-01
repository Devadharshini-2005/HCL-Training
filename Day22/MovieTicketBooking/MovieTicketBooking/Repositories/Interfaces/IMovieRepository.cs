using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces;

public interface IMovieRepository
{
    Task<Movie> AddAsync(Movie movie);

    Task<List<Movie>> GetAllAsync(
        int pageNumber,
        int pageSize,
        string? title);

    Task<Movie?> GetByIdAsync(int movieId);

    Task<List<Movie>> SearchByTitleAsync(string title);

    Task UpdateAsync(Movie movie);

    Task DeleteAsync(Movie movie);

    Task<bool> HasShowsAsync(int movieId);
}