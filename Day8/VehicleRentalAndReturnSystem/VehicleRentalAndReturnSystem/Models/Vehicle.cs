namespace VehicleRentalSystem.Models;

public class Vehicle
{
    public string RegistrationNumber { get; set; }
    public string Model { get; set; }
    public string Category { get; set; }
    public decimal DailyRate { get; set; }
    public bool IsAvailable { get; set; }

    public Vehicle(string registrationNumber, string model,
        string category, decimal dailyRate)
    {
        RegistrationNumber = registrationNumber;
        Model = model;
        Category = category;
        DailyRate = dailyRate;
        IsAvailable = true;
    }

    public override string ToString() =>
        $"{RegistrationNumber,-10} {Model,-15} {Category,-8} " +
        $"Rs.{DailyRate,-8:F2} {(IsAvailable ? "Available" : "Rented")}";
}