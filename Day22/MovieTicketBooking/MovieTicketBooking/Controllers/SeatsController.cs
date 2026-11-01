using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Seats;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SeatsController : ControllerBase
{
    private readonly ISeatService _seatService;

    public SeatsController(ISeatService seatService)
    {
        _seatService = seatService;
    }

    // POST: api/seats
    [HttpPost]
    public async Task<IActionResult> CreateSeat(
        [FromBody] CreateSeatRequest request)
    {
        try
        {
            var seat = await _seatService.CreateSeatAsync(request);

            return CreatedAtAction(
                nameof(GetSeatById),
                new { id = seat.SeatId },
                seat);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // POST: api/seats/bulk
    [HttpPost("bulk")]
    public async Task<IActionResult> CreateSeatsBulk(
        [FromBody] BulkCreateSeatsRequest request)
    {
        try
        {
            var seats = await _seatService.CreateSeatsBulkAsync(request);

            return Ok(seats);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // GET: api/seats/screen/{screenId}
    [HttpGet("screen/{screenId:int}")]
    public async Task<IActionResult> GetSeatsByScreenId(int screenId)
    {
        try
        {
            var seats = await _seatService
                .GetSeatsByScreenIdAsync(screenId);

            return Ok(seats);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // GET: api/seats/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetSeatById(int id)
    {
        var seat = await _seatService.GetSeatByIdAsync(id);

        if (seat == null)
        {
            return NotFound(new { message = "Seat not found." });
        }

        return Ok(seat);
    }

    // PUT: api/seats/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateSeat(
        int id,
        [FromBody] UpdateSeatRequest request)
    {
        try
        {
            var seat = await _seatService.UpdateSeatAsync(id, request);

            if (seat == null)
            {
                return NotFound(new { message = "Seat not found." });
            }

            return Ok(seat);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE: api/seats/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteSeat(int id)
    {
        try
        {
            bool deleted = await _seatService.DeleteSeatAsync(id);

            if (!deleted)
            {
                return NotFound(new { message = "Seat not found." });
            }

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}