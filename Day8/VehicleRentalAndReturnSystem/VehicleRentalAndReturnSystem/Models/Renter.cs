namespace VehicleRentalSystem.Models;

public class Renter
{
    public string RenterId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string ContactNumber { get; set; }

    public Renter(string renterId, string name,
        string email, string contactNumber)
    {
        RenterId = renterId;
        Name = name;
        Email = email;
        ContactNumber = contactNumber;
    }
}