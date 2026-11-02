using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Shows;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers;

[ApiController]
[Route("api/shows")]
public class ShowsController : ControllerBase
{
    private readonly IShowService _showService;

    public ShowsController(IShowService showService)
    {
        _showService = showService;
    }

    // POST: api/shows
    [Authorize(Roles = "Admin")]

    [HttpPost]
    public async Task<IActionResult> CreateShow(
        [FromBody] CreateShowRequest request)
    {
        try
        {
            var show = await _showService.CreateShowAsync(request);

            return CreatedAtAction(
                nameof(GetShowById),
                new { id = show.ShowId },
                show);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // GET: api/shows

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllShows(
        [FromQuery] DateTime? date,
        [FromQuery] int? movieId,
        [FromQuery] int? theatreId)
    {
        var shows = await _showService.GetAllShowsAsync(
            date, movieId, theatreId);

        return Ok(shows);
    }

    // GET: api/shows/{id}

    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetShowById(int id)
    {
        var show = await _showService.GetShowByIdAsync(id);

        if (show == null)
        {
            return NotFound(new { message = "Show not found." });
        }

        return Ok(show);
    }

    // GET: api/shows/movie/{movieId}
    [AllowAnonymous]
    [HttpGet("movie/{movieId:int}")]
    public async Task<IActionResult> GetShowsByMovie(int movieId)
    {
        var shows = await _showService.GetShowsByMovieIdAsync(movieId);

        return Ok(shows);
    }

    // GET: api/shows/theatre/{theatreId}
    [AllowAnonymous]
    [HttpGet("theatre/{theatreId:int}")]
    public async Task<IActionResult> GetShowsByTheatre(int theatreId)
    {
        var shows = await _showService.GetShowsByTheatreIdAsync(theatreId);

        return Ok(shows);
    }

    // GET: api/shows/search?date=2026-11-01
    [AllowAnonymous]
    [HttpGet("search")]
    public async Task<IActionResult> SearchShowsByDate(
        [FromQuery] DateTime date)
    {
        var shows = await _showService.SearchShowsByDateAsync(date);

        return Ok(shows);
    }

    // PUT: api/shows/{id}
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateShow(
        int id,
        [FromBody] UpdateShowRequest request)
    {
        try
        {
            var show = await _showService.UpdateShowAsync(id, request);

            if (show == null)
            {
                return NotFound(new { message = "Show not found." });
            }

            return Ok(show);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE: api/shows/{id}
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteShow(int id)
    {
        try
        {
            bool deleted = await _showService.DeleteShowAsync(id);

            if (!deleted)
            {
                return NotFound(new { message = "Show not found." });
            }

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // GET: api/shows/{showId}/seats
    [HttpGet("{showId:int}/seats")]
    public async Task<IActionResult> GetShowSeats(int showId)
    {
        try
        {
            var seats = await _showService.GetShowSeatsAsync(showId);
            return Ok(seats);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // GET: api/shows/{showId}/seats/available
    [HttpGet("{showId:int}/seats/available")]
    public async Task<IActionResult> GetAvailableShowSeats(int showId)
    {
        try
        {
            var seats =
                await _showService.GetAvailableShowSeatsAsync(showId);

            return Ok(seats);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // GET: api/shows/{showId}/seats/booked
    [HttpGet("{showId:int}/seats/booked")]
    public async Task<IActionResult> GetBookedShowSeats(int showId)
    {
        try
        {
            var seats =
                await _showService.GetBookedShowSeatsAsync(showId);

            return Ok(seats);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}