using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Movies;
using MovieTicketBooking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;


namespace MovieTicketBooking.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    // POST: api/movies
    // Create a new movie
    [HttpPost]
    public async Task<IActionResult> CreateMovie(
        [FromBody] CreateMovieRequest request)
    {
        var movie = await _movieService.CreateMovieAsync(request);

        return CreatedAtAction(
            nameof(GetMovieById),
            new { id = movie.MovieId },
            movie);
    }

    // GET: api/movies
    // Get all movies with pagination and optional title filter
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllMovies(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? title = null)
    {
        var movies = await _movieService.GetAllMoviesAsync(
            pageNumber,
            pageSize,
            title);

        return Ok(movies);
    }

    // GET: api/movies/1
    // Get movie by ID
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetMovieById(int id)
    {
        var movie = await _movieService.GetMovieByIdAsync(id);

        if (movie == null)
        {
            return NotFound(new
            {
                message = "Movie not found."
            });
        }

        return Ok(movie);
    }

    // GET: api/movies/search?title=leo
    // Search movies by title
    [HttpGet("search")]
    public async Task<IActionResult> SearchMovies(
        [FromQuery] string title)
    {
        var movies = await _movieService.SearchMoviesAsync(title);

        return Ok(movies);
    }

    // PUT: api/movies/1
    // Update movie

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateMovie(
        int id,
        [FromBody] UpdateMovieRequest request)
    {
        var movie = await _movieService.UpdateMovieAsync(id, request);

        if (movie == null)
        {
            return NotFound(new
            {
                message = "Movie not found."
            });
        }

        return Ok(movie);
    }

    // DELETE: api/movies/1
    // Delete movie

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        try
        {
            bool deleted = await _movieService.DeleteMovieAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Movie not found."
                });
            }

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }
}