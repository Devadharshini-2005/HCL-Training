using VehicleRentalSystem.Models;
using VehicleRentalSystem.Services;
using VehicleRentalSystem.Payments;

namespace VehicleRentalSystem;

internal class Program
{
    static void Main()
    {
        RentalService service = new RentalService();
        bool running = true;

        // Value type assignment demonstration.
        int firstValue = 10;
        int copiedValue = firstValue;
        copiedValue = 20;

        Console.WriteLine(
            $"Value type: original={firstValue}, copy={copiedValue}");

        // Reference type assignment demonstration.
        Vehicle originalVehicle = service.GetVehicles()[0];
        Vehicle referencedVehicle = originalVehicle;
        referencedVehicle.Model = "Updated Swift";

        Console.WriteLine(
            $"Reference type: original model={originalVehicle.Model}");

        // Record equality and 'with' demonstration.
        var summary1 = new RentalSummary(
            "C001", "TN01AB1234", 2, 3000m, 600m, 3600m);

        var summary2 = summary1 with { Charge = 3200m };

        Console.WriteLine(
            $"Record equality: {summary1 == summary2}");

        // Restore the sample vehicle name.
        originalVehicle.Model = "Swift";

        while (running)
        {
            Console.WriteLine("\n===== VEHICLE RENTAL SYSTEM =====");
            Console.WriteLine("1. List vehicles");
            Console.WriteLine("2. Search vehicles");
            Console.WriteLine("3. Sort vehicles by rate");
            Console.WriteLine("4. Register renter");
            Console.WriteLine("5. Rent vehicle");
            Console.WriteLine("6. Return vehicle");
            Console.WriteLine("7. Process payment");
            Console.WriteLine("8. View contract");
            Console.WriteLine("9. Exit");
            Console.Write("Choose option: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Enter a valid menu number.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    service.ListVehicles();
                    break;

                case 2:
                    Console.Write("Enter model or category: ");
                    string keyword = Console.ReadLine() ?? "";
                    service.SearchVehicles(keyword);
                    break;

                case 3:
                    service.SortVehiclesByRate();
                    break;

                case 4:
                    RegisterRenter(service);
                    break;

                case 5:
                    RentVehicle(service);
                    break;

                case 6:
                    ReturnVehicle(service);
                    break;

                case 7:
                    ProcessPayment(service);
                    break;

                case 8:
                    ViewContract(service);
                    break;

                case 9:
                    running = false;
                    Console.WriteLine("Thank you!");
                    break;

                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    static void RegisterRenter(RentalService service)
    {
        Console.Write("Name: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("Email: ");
        string email = Console.ReadLine() ?? "";

        Console.Write("10-digit contact number: ");
        string contact = Console.ReadLine() ?? "";

        try
        {
            service.RegisterRenter(name, email, contact);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Registration failed: {ex.Message}");
        }
    }

    static void RentVehicle(RentalService service)
    {
        service.ListVehicles();

        Console.Write("Vehicle registration: ");
        string registrationInput = Console.ReadLine() ?? "";

        // out parameter safely validates the registration.
        if (!service.TryParseRegistration(
                registrationInput, out string registration))
        {
            Console.WriteLine("Invalid registration number.");
            return;
        }

        Console.Write("Renter ID: ");
        string renterId = Console.ReadLine() ?? "";

        if (service.FindRenter(renterId) is not Renter)
        {
            Console.WriteLine("Register the renter first.");
            return;
        }

        Console.Write("Start date (yyyy-MM-dd): ");
        if (!DateTime.TryParseExact(
                Console.ReadLine(),
                "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out DateTime start))
        {
            Console.WriteLine("Invalid start date.");
            return;
        }

        Console.Write("End date (yyyy-MM-dd): ");
        if (!DateTime.TryParseExact(
                Console.ReadLine(),
                "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out DateTime end))
        {
            Console.WriteLine("Invalid end date.");
            return;
        }

        service.RentVehicle(registration, renterId, start, end);
    }

    static void ReturnVehicle(RentalService service)
    {
        Console.Write("Contract ID: ");
        string input = Console.ReadLine() ?? "";

        if (!service.TryParseContractId(input, out string id))
        {
            Console.WriteLine("Invalid contract ID.");
            return;
        }

        Console.Write("Actual return date (yyyy-MM-dd): ");

        if (!DateTime.TryParseExact(
                Console.ReadLine(),
                "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out DateTime returnDate))
        {
            Console.WriteLine("Invalid return date.");
            return;
        }

        Console.Write("Damage fee (Rs.): ");

        if (!decimal.TryParse(Console.ReadLine(), out decimal damageFee)
            || damageFee < 0)
        {
            Console.WriteLine("Invalid damage fee.");
            return;
        }

        Console.Write("Mileage travelled (km): ");

        if (!decimal.TryParse(Console.ReadLine(), out decimal mileage)
            || mileage < 0)
        {
            Console.WriteLine("Invalid mileage.");
            return;
        }

        service.ReturnVehicle(id, returnDate, damageFee, mileage);
    }

    static void ProcessPayment(RentalService service)
    {
        Console.Write("Contract ID: ");
        string input = Console.ReadLine() ?? "";

        if (!service.TryParseContractId(input, out string id))
        {
            Console.WriteLine("Invalid contract ID.");
            return;
        }

        RentalContract? contract = service.FindContract(id);

        if (contract is null)
        {
            Console.WriteLine("Contract not found.");
            return;
        }

        if (contract.IsReturned)
        {
            Console.WriteLine("This contract has already been closed.");
            return;
        }

        RentalSummary summary = service.CalculateBill(contract);

        service.DisplaySummary(in summary);

        // 'with' creates a modified copy of the record.
        RentalSummary adjusted = summary with
        {
            Total = summary.Charge + summary.Deposit
        };

        Console.WriteLine("\n1. Cash");
        Console.WriteLine("2. Card");
        Console.Write("Payment method: ");

        if (!int.TryParse(Console.ReadLine(), out int method))
        {
            Console.WriteLine("Invalid payment method.");
            return;
        }

        // Interface reference supports both implementations.
        IPaymentProcessor? processor = method switch
        {
            1 => new CashPaymentProcessor(),
            2 => new CardPaymentProcessor(),
            _ => null
        };

        if (processor is null)
        {
            Console.WriteLine("Choose 1 or 2.");
            return;
        }

        // Pattern matching verifies the selected processor.
        if (processor is CashPaymentProcessor cash)
        {
            IPaymentProcessor cashInterface = cash;
            cashInterface.ProcessPayment(adjusted.Total);
        }
        else if (processor is CardPaymentProcessor card)
        {
            IPaymentProcessor cardInterface = card;
            cardInterface.ProcessPayment(adjusted.Total);
        }
    }

    static void ViewContract(RentalService service)
    {
        Console.Write("Contract ID: ");
        string input = Console.ReadLine() ?? "";

        if (!service.TryParseContractId(input, out string id))
        {
            Console.WriteLine("Contract not found.");
            return;
        }

        RentalContract? contract = service.FindContract(id);

        if (contract is null)
            return;

        Console.WriteLine("\n--- RENTAL CONTRACT ---");
        Console.WriteLine($"Contract ID: {contract.ContractId}");
        Console.WriteLine($"Renter: {contract.Renter.Name}");
        Console.WriteLine($"Vehicle: {contract.Vehicle.Model}");
        Console.WriteLine(
            $"Registration: {contract.Vehicle.RegistrationNumber}");
        Console.WriteLine($"Category: {contract.Vehicle.Category}");
        Console.WriteLine($"Start: {contract.Period.StartDate:yyyy-MM-dd}");
        Console.WriteLine($"End: {contract.Period.EndDate:yyyy-MM-dd}");
        Console.WriteLine($"Returned: {contract.IsReturned}");

        RentalSummary summary = service.CalculateBill(contract);
        service.DisplaySummary(in summary);
    }
}