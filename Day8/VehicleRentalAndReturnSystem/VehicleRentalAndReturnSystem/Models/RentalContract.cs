namespace VehicleRentalSystem.Models;

public abstract class RentalContract
{
    public string ContractId { get; }
    public Renter Renter { get; }
    public Vehicle Vehicle { get; }
    public RentalPeriod Period { get; }
    public bool IsReturned { get; set; }

    protected RentalContract(string contractId, Renter renter,
        Vehicle vehicle, RentalPeriod period)
    {
        ContractId = contractId;
        Renter = renter;
        Vehicle = vehicle;
        Period = period;
        IsReturned = false;
    }

    public virtual decimal CalculateCharge()
    {
        return Vehicle.DailyRate * Period.Days;
    }
}