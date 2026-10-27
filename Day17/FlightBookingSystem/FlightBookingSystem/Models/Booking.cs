namespace FlightBookingSystem.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public string PassengerName { get; set; } = "";
        public int FlightId { get; set; }
        public int Seats { get; set; }
        public decimal TotalPrice { get; set; }
    }
}