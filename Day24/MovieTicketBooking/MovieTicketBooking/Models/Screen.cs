using System.Collections.Generic;

namespace MovieTicketBooking.Models;

public class Screen
{
    public int ScreenId { get; set; }
    public int TheatreId { get; set; }
    public required string ScreenName { get; set; }

    public Theatre? Theatre { get; set; }

    public ICollection<Seat> Seats { get; set; } = new List<Seat>();
    public ICollection<Show> Shows { get; set; } = new List<Show>();
}