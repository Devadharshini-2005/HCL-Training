namespace FlightBookingSystem.Models
{
    public class Flight
    {
        public int FlightId { get; set; }
        public string FlightName { get; set; } = "";
        public string Source { get; set; } = "";
        public string Destination { get; set; } = "";
        public decimal Price { get; set; }
    }
}