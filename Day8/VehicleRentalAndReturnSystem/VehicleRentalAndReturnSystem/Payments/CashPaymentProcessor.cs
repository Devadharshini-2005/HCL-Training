namespace VehicleRentalSystem.Payments;

public class CashPaymentProcessor : IPaymentProcessor
{
    public bool ProcessPayment(decimal amount)
    {
        Console.WriteLine(
            $"Cash payment received: Rs.{amount:F2}");
        return true;
    }
}