using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Bookings;
using MovieTicketBooking.Models;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    // POST: api/bookings
    [HttpPost]
    public async Task<IActionResult> CreateBooking(
    CreateBookingRequest request)
    {
        var booking = await _bookingService.CreateBookingAsync(request);

        return CreatedAtAction(
            nameof(GetBookingById),
            new { id = booking.BookingId },
            booking);
    }

    // GET: api/bookings?showId=1&status=Confirmed
    [HttpGet]
    public async Task<IActionResult> GetAllBookings(
        [FromQuery] int? showId,
        [FromQuery] BookingStatus? status)
    {
        var bookings = await _bookingService.GetAllBookingsAsync(
            showId,
            status);

        return Ok(bookings);
    }

    // GET: api/bookings/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookingById(int id)
    {
        var booking = await _bookingService.GetBookingByIdAsync(id);

        if (booking == null)
        {
            return NotFound(new { message = "Booking not found." });
        }

        return Ok(booking);
    }

    // GET: api/bookings/show/1
    [HttpGet("show/{showId}")]
    public async Task<IActionResult> GetBookingsByShowId(int showId)
    {
        var bookings = await _bookingService.GetBookingsByShowIdAsync(
            showId);

        return Ok(bookings);
    }

    // PUT: api/bookings/1/cancel
    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> CancelBooking(int id)
    {
        var booking = await _bookingService.CancelBookingAsync(id);

        if (booking == null)
            return NotFound();

        return Ok(booking);
    }
}