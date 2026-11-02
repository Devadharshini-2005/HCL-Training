
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using VehicleRentalSystem.Models;

namespace VehicleRentalSystem.Services
{
    public class RentalService
    {
        private readonly Vehicle[] vehicles =
        {
            new Vehicle("TN01AB1234", "Swift", "Car", 1500m),
            new Vehicle("TN02CD5678", "Innova", "Car", 2500m),
            new Vehicle("TN03EF9012", "Baleno", "Car", 1800m),
            new Vehicle("TN04GH3456", "Activa", "Bike", 400m),
            new Vehicle("TN05IJ7890", "Pulsar", "Bike", 600m)
        };

        private readonly List<Renter> renters = new List<Renter>();
        private readonly List<RentalContract> contracts =
            new List<RentalContract>();

        private int renterCount = 0;
        private int rentalCount = 0;

        // Return the vehicle array.
        public Vehicle[] GetVehicles()
        {
            return vehicles;
        }

        // 1. List all vehicles.
        public void ListVehicles()
        {
            Console.WriteLine("\n--- AVAILABLE VEHICLES ---");

            foreach (Vehicle vehicle in vehicles)
            {
                Console.WriteLine(
                    $"{vehicle.RegistrationNumber} | " +
                    $"{vehicle.Model} | {vehicle.Category} | " +
                    $"Rs.{vehicle.DailyRate:F2} | " +
                    $"{(vehicle.IsAvailable ? "Available" : "Rented")}");
            }
        }

        // 2. Search by model or category without case sensitivity.
        public void SearchVehicles(string keyword)
        {
            bool found = false;

            foreach (Vehicle vehicle in vehicles)
            {
                if (vehicle.Model.Contains(
                        keyword ?? "",
                        StringComparison.OrdinalIgnoreCase) ||
                    vehicle.Category.Equals(
                        keyword ?? "",
                        StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(vehicle);
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("No matching vehicles found.");
            }
        }

        // 3. Sort vehicles by daily rate.
        public void SortVehiclesByRate()
        {
            Array.Sort(
                vehicles,
                (first, second) =>
                    first.DailyRate.CompareTo(second.DailyRate));

            Console.WriteLine("\nVehicles sorted by daily rate:");
            ListVehicles();
        }

        // 4. Register a renter.
        public Renter RegisterRenter(
            string name,
            string email,
            string contact)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name is required.");
            }

            email = email?.Trim() ?? "";
            contact = contact?.Trim() ?? "";

            if (!Regex.IsMatch(
                    email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new ArgumentException(
                    "Enter a valid email address.");
            }

            if (!Regex.IsMatch(contact, @"^\d{10}$"))
            {
                throw new ArgumentException(
                    "Contact number must contain 10 digits.");
            }

            renterCount++;

            Renter renter = new Renter(
                $"R{renterCount:D3}",
                name.Trim(),
                email,
                contact);

            renters.Add(renter);

            Console.WriteLine(
                $"Renter registered successfully. ID: {renter.RenterId}");

            return renter;
        }

        // Find a renter by ID.
        public Renter? FindRenter(string id)
        {
            foreach (Renter renter in renters)
            {
                if (renter.RenterId.Equals(
                        id?.Trim(),
                        StringComparison.OrdinalIgnoreCase))
                {
                    return renter;
                }
            }

            return null;
        }

        // 5. Validate a vehicle registration using out.
        public bool TryParseRegistration(
            string? input,
            out string registration)
        {
            registration = input?.Trim().ToUpperInvariant() ?? "";

            foreach (Vehicle vehicle in vehicles)
            {
                if (vehicle.RegistrationNumber.Equals(
                        registration,
                        StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        // Validate a contract ID using out.
        public bool TryParseContractId(
            string? input,
            out string id)
        {
            id = input?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(id))
            {
                return false;
            }

            foreach (RentalContract contract in contracts)
            {
                if (contract.ContractId.Equals(
                        id,
                        StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        // 6. Overload 1: calculate charge using days.
        public decimal CalculateCharge(Vehicle vehicle, int days)
        {
            if (days <= 0)
            {
                throw new ArgumentException(
                    "Rental days must be greater than zero.");
            }

            return vehicle.DailyRate * days;
        }

        // 7. Overload 2: calculate charge using RentalPeriod.
        public decimal CalculateCharge(
            Vehicle vehicle,
            RentalPeriod period)
        {
            return CalculateCharge(vehicle, period.Days);
        }

        // 8. Rent an available vehicle.
        public RentalContract? RentVehicle(
            string registration,
            string renterId,
            DateTime start,
            DateTime end)
        {
            Vehicle? vehicle = null;

            foreach (Vehicle item in vehicles)
            {
                if (item.RegistrationNumber.Equals(
                        registration,
                        StringComparison.OrdinalIgnoreCase))
                {
                    vehicle = item;
                    break;
                }
            }

            if (vehicle == null)
            {
                Console.WriteLine("Vehicle not found.");
                return null;
            }

            if (!vehicle.IsAvailable)
            {
                Console.WriteLine(
                    "This vehicle is already rented.");
                return null;
            }

            Renter? renter = FindRenter(renterId);

            if (renter == null)
            {
                Console.WriteLine("Renter not found.");
                return null;
            }

            RentalPeriod period;

            try
            {
                period = new RentalPeriod(start, end);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(
                    $"Invalid rental period: {ex.Message}");
                return null;
            }

            // ref passes the rental counter by reference.
            IncrementRentalCount(ref rentalCount);

            string contractId = $"C{rentalCount:D3}";

            // Switch expression creates the correct contract subclass.
            RentalContract contract =
                vehicle.Category.ToLowerInvariant() switch
                {
                    "car" => new CarRental(
                        contractId, renter, vehicle, period),

                    "bike" => new BikeRental(
                        contractId, renter, vehicle, period),

                    _ => throw new InvalidOperationException(
                        "Unsupported vehicle category.")
                };

            vehicle.IsAvailable = false;
            contracts.Add(contract);

            Console.WriteLine("\nVehicle rented successfully!");
            Console.WriteLine($"Contract ID: {contract.ContractId}");

            RentalSummary bill = CalculateBill(contract);
            DisplaySummary(in bill);

            return contract;
        }

        // ref example.
        private static void IncrementRentalCount(ref int count)
        {
            count++;
        }

        // Find a contract.
        public RentalContract? FindContract(string id)
        {
            foreach (RentalContract contract in contracts)
            {
                if (contract.ContractId.Equals(
                        id?.Trim(),
                        StringComparison.OrdinalIgnoreCase))
                {
                    return contract;
                }
            }

            return null;
        }

        // 9. Calculate bill and deposit.
        public RentalSummary CalculateBill(
            RentalContract contract)
        {
            decimal charge = contract.CalculateCharge();
            decimal deposit = charge * 0.20m;

            return new RentalSummary(
                contract.ContractId,
                contract.Vehicle.RegistrationNumber,
                contract.Period.Days,
                charge,
                deposit,
                charge + deposit);
        }

        // in example: read-only summary parameter.
        public void DisplaySummary(in RentalSummary summary)
        {
            Console.WriteLine("\n--- RENTAL BILL ---");
            Console.WriteLine($"Contract ID: {summary.ContractId}");
            Console.WriteLine(
                $"Registration: {summary.RegistrationNumber}");
            Console.WriteLine($"Days: {summary.Days}");
            Console.WriteLine(
                $"Rental charge: Rs.{summary.Charge:F2}");
            Console.WriteLine(
                $"Deposit: Rs.{summary.Deposit:F2}");
            Console.WriteLine(
                $"Total: Rs.{summary.Total:F2}");
        }

        // 10. Return a vehicle and calculate the final bill.
        public void ReturnVehicle(
            string contractId,
            DateTime returnDate,
            decimal damageFee,
            decimal mileage)
        {
            RentalContract? contract = FindContract(contractId);

            if (contract == null)
            {
                Console.WriteLine("Contract not found.");
                return;
            }

            if (contract.IsReturned)
            {
                Console.WriteLine(
                    "This vehicle has already been returned.");
                return;
            }

            if (returnDate.Date < contract.Period.EndDate)
            {
                Console.WriteLine(
                    "Return date cannot be before the booked end date.");
                return;
            }

            if (damageFee < 0 || mileage < 0)
            {
                Console.WriteLine(
                    "Damage fee and mileage cannot be negative.");
                return;
            }

            int extraDays = (
                returnDate.Date - contract.Period.EndDate).Days;

            decimal lateFee =
                extraDays * contract.Vehicle.DailyRate * 1.5m;

            // Bonus: mileage fee for bikes after 100 km.
            decimal mileageFee = 0m;

            if (contract.Vehicle.Category.Equals(
                    "Bike",
                    StringComparison.OrdinalIgnoreCase))
            {
                mileageFee = Math.Max(0m, mileage - 100m) * 2m;
            }

            decimal originalCharge = contract.CalculateCharge();
            decimal deposit = originalCharge * 0.20m;

            decimal finalTotal =
                originalCharge + lateFee + damageFee + mileageFee;

            decimal balance = finalTotal - deposit;

            contract.IsReturned = true;
            contract.Vehicle.IsAvailable = true;

            Console.WriteLine("\n--- FINAL RETURN BILL ---");
            Console.WriteLine(
                $"Original rental charge: Rs.{originalCharge:F2}");
            Console.WriteLine(
                $"Late fee ({extraDays} extra days): Rs.{lateFee:F2}");
            Console.WriteLine($"Damage fee: Rs.{damageFee:F2}");
            Console.WriteLine($"Mileage fee: Rs.{mileageFee:F2}");
            Console.WriteLine($"Deposit credited: Rs.{deposit:F2}");
            Console.WriteLine(
                $"Final charges: Rs.{finalTotal:F2}");
            Console.WriteLine(
                $"Balance after deposit: Rs.{balance:F2}");
        }
    }
}
