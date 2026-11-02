namespace VehicleRentalSystem.Models;

public readonly record struct RentalSummary(
    string ContractId,
    string RegistrationNumber,
    int Days,
    decimal Charge,
    decimal Deposit,
    decimal Total);