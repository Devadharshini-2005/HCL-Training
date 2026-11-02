namespace VehicleRentalSystem.Models;

public class CarRental : RentalContract
{
    public CarRental(string id, Renter renter,
        Vehicle vehicle, RentalPeriod period)
        : base(id, renter, vehicle, period)
    {
    }

    public override decimal CalculateCharge()
    {
        return base.CalculateCharge() + 200m * Period.Days;
    }
}