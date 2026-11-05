namespace MovieTicketBooking.Models;

public class ShowSeat
{
    public int ShowSeatId { get; set; }
    public int ShowId { get; set; }
    public int SeatId { get; set; }
    public ShowSeatStatus Status { get; set; } = ShowSeatStatus.Available;

    public Show? Show { get; set; }
    public Seat? Seat { get; set; }
}

public enum ShowSeatStatus
{
    Available,
    Booked
}