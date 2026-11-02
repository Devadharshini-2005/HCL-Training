namespace VehicleRentalSystem.Models;

public readonly struct RentalPeriod
{
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }

    public RentalPeriod(DateTime startDate, DateTime endDate)
    {
        if (endDate.Date <= startDate.Date)
            throw new ArgumentException(
                "End date must be after start date.");

        StartDate = startDate.Date;
        EndDate = endDate.Date;
    }

    public int Days => (EndDate - StartDate).Days;
}