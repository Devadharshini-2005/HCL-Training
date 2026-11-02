namespace VehicleRentalSystem.Payments;

public class CardPaymentProcessor : IPaymentProcessor
{
    bool IPaymentProcessor.ProcessPayment(decimal amount)
    {
        Console.WriteLine(
            $"Simulated card payment: Rs.{amount:F2}");
        return true;
    }
}