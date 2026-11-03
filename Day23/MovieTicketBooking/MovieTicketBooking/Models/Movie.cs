using System;
using System.Collections.Generic;

namespace MovieTicketBooking.Models;

public class Movie
{
    public int MovieId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public int DurationMinutes { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public required string Language { get; set; }
    public required string Genre { get; set; }

    public ICollection<Show> Shows { get; set; } = new List<Show>();
}