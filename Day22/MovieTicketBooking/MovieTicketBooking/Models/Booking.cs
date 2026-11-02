using System;
using System.Collections.Generic;

namespace MovieTicketBooking.Models;

public class Booking
{
    public int BookingId { get; set; }
    public required string BookingReference { get; set; }
    public int ShowId { get; set; }
    public required string CustomerName { get; set; }
    public required string CustomerEmail { get; set; }
    public required string CustomerPhone { get; set; }
    public DateTime BookingDate { get; set; }
    public decimal TotalAmount { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Confirmed;

    public Show? Show { get; set; }

    public ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();

    public int? UserId { get; set; }

    public User? User { get; set; }

}

public enum BookingStatus
{
    Confirmed,
    Cancelled
}