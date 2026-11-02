using System;
using System.Collections.Generic;
using System.Text;


namespace VehicleRentalSystem.Models;

public readonly struct RentalReceipt
{
    public string ContractId { get; }
    public string VehicleNumber { get; }
    public decimal AmountDue { get; }

    public RentalReceipt(string contractId,
        string vehicleNumber, decimal amountDue)
    {
        ContractId = contractId;
        VehicleNumber = vehicleNumber;
        AmountDue = amountDue;
    }
}

