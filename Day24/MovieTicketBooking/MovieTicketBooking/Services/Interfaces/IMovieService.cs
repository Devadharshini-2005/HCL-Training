using MovieTicketBooking.DTOs.Movies;

namespace MovieTicketBooking.Services.Interfaces;

public interface IMovieService
{
    Task<MovieResponse> CreateMovieAsync(CreateMovieRequest request);

    Task<List<MovieResponse>> GetAllMoviesAsync(
        int pageNumber,
        int pageSize,
        string? title);

    Task<MovieResponse?> GetMovieByIdAsync(int movieId);

    Task<List<MovieResponse>> SearchMoviesAsync(string title);

    Task<MovieResponse?> UpdateMovieAsync(
        int movieId,
        UpdateMovieRequest request);

    Task<bool> DeleteMovieAsync(int movieId);
}