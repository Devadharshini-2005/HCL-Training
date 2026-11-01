using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Screens;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScreensController : ControllerBase
{
    private readonly IScreenService _screenService;

    public ScreensController(IScreenService screenService)
    {
        _screenService = screenService;
    }

    // POST: api/screens
    // Create a new screen
    [HttpPost]
    public async Task<IActionResult> CreateScreen(
        CreateScreenRequest request)
    {
        try
        {
            var screen =
                await _screenService.CreateScreenAsync(request);

            return CreatedAtAction(
                nameof(GetScreenById),
                new { id = screen.ScreenId },
                screen);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // GET: api/screens?theatreId=1
    // Get all screens with optional theatre filter
    [HttpGet]
    public async Task<IActionResult> GetAllScreens(
        [FromQuery] int? theatreId)
    {
        var screens =
            await _screenService.GetAllScreensAsync(theatreId);

        return Ok(screens);
    }

    // GET: api/screens/1
    // Get screen by ID
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetScreenById(int id)
    {
        var screen =
            await _screenService.GetScreenByIdAsync(id);

        if (screen == null)
        {
            return NotFound(new
            {
                message = $"Screen with ID {id} was not found."
            });
        }

        return Ok(screen);
    }

    // GET: api/screens/theatre/1
    // Get screens by theatre ID
    [HttpGet("theatre/{theatreId:int}")]
    public async Task<IActionResult> GetScreensByTheatreId(
        int theatreId)
    {
        var screens =
            await _screenService.GetScreensByTheatreIdAsync(theatreId);

        return Ok(screens);
    }

    // PUT: api/screens/1
    // Update screen
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateScreen(
        int id,
        UpdateScreenRequest request)
    {
        try
        {
            var screen =
                await _screenService.UpdateScreenAsync(id, request);

            if (screen == null)
            {
                return NotFound(new
                {
                    message = $"Screen with ID {id} was not found."
                });
            }

            return Ok(screen);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE: api/screens/1
    // Delete screen
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteScreen(int id)
    {
        try
        {
            bool isDeleted =
                await _screenService.DeleteScreenAsync(id);

            if (!isDeleted)
            {
                return NotFound(new
                {
                    message = $"Screen with ID {id} was not found."
                });
            }

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}