namespace VehicleRentalSystem.Models;

public class BikeRental : RentalContract
{
    public BikeRental(string id, Renter renter,
        Vehicle vehicle, RentalPeriod period)
        : base(id, renter, vehicle, period)
    {
    }

    public override decimal CalculateCharge()
    {
        return base.CalculateCharge() + 50m * Period.Days;
    }
}