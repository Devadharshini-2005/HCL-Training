using MovieTicketBooking.DTOs.Movies;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;

    public MovieService(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    // Create a movie
    public async Task<MovieResponse> CreateMovieAsync(
        CreateMovieRequest request)
    {
        var movie = new Movie
        {
            Title = request.Title,
            Description = request.Description,
            DurationMinutes = request.DurationMinutes,
            ReleaseDate = request.ReleaseDate,
            Language = request.Language,
            Genre = request.Genre
        };

        var createdMovie = await _movieRepository.AddAsync(movie);

        return MapToResponse(createdMovie);
    }

    // Get all movies with pagination
    public async Task<List<MovieResponse>> GetAllMoviesAsync(
        int pageNumber,
        int pageSize,
        string? title)
    {
        if (pageNumber < 1)
            throw new ArgumentException("Page number must be greater than zero.");

        if (pageSize < 1)
            throw new ArgumentException("Page size must be greater than zero.");

        var movies = await _movieRepository.GetAllAsync(
            pageNumber,
            pageSize,
            title);

        return movies.Select(MapToResponse).ToList();
    }

    // Get movie by ID
    public async Task<MovieResponse?> GetMovieByIdAsync(int movieId)
    {
        var movie = await _movieRepository.GetByIdAsync(movieId);

        if (movie == null)
            return null;

        return MapToResponse(movie);
    }

    // Search movies by title
    public async Task<List<MovieResponse>> SearchMoviesAsync(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Search title is required.");

        var movies = await _movieRepository.SearchByTitleAsync(title);

        return movies.Select(MapToResponse).ToList();
    }

    // Update movie
    public async Task<MovieResponse?> UpdateMovieAsync(
        int movieId,
        UpdateMovieRequest request)
    {
        var movie = await _movieRepository.GetByIdAsync(movieId);

        if (movie == null)
            return null;

        movie.Title = request.Title;
        movie.Description = request.Description;
        movie.DurationMinutes = request.DurationMinutes;
        movie.ReleaseDate = request.ReleaseDate;
        movie.Language = request.Language;
        movie.Genre = request.Genre;

        await _movieRepository.UpdateAsync(movie);

        return MapToResponse(movie);
    }

    // Delete movie
    public async Task<bool> DeleteMovieAsync(int movieId)
    {
        var movie = await _movieRepository.GetByIdAsync(movieId);

        if (movie == null)
            return false;

        bool hasShows = await _movieRepository.HasShowsAsync(movieId);

        if (hasShows)
        {
            throw new InvalidOperationException(
                "This movie cannot be deleted because shows are associated with it.");
        }

        await _movieRepository.DeleteAsync(movie);

        return true;
    }

    // Convert Movie entity into MovieResponse DTO
    private static MovieResponse MapToResponse(Movie movie)
    {
        return new MovieResponse
        {
            MovieId = movie.MovieId,
            Title = movie.Title,
            Description = movie.Description,
            DurationMinutes = movie.DurationMinutes,
            ReleaseDate = movie.ReleaseDate,
            Language = movie.Language,
            Genre = movie.Genre
        };
    }
}