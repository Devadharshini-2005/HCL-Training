using System.Collections.Generic;

namespace MovieTicketBooking.Models;

public class Theatre
{
    public int TheatreId { get; set; }
    public required string Name { get; set; }
    public required string Location { get; set; }

    public ICollection<Screen> Screens { get; set; } = new List<Screen>();
}