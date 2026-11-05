using System.Collections.Generic;

namespace MovieTicketBooking.Models;

public class Seat
{
    public int SeatId { get; set; }
    public int ScreenId { get; set; }
    public required string RowName { get; set; }
    public int SeatNumber { get; set; }
    public required string SeatType { get; set; }

    public Screen? Screen { get; set; }

    public ICollection<ShowSeat> ShowSeats { get; set; } = new List<ShowSeat>();
    public ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();
}