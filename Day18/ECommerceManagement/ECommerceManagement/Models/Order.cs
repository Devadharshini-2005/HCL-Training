namespace ECommerceManagement.Models;

public class Order
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = "Placed";
    public decimal TotalAmount { get; set; }

    public Order(int customerId, DateTime orderDate, decimal totalAmount)
    {
        OrderId ++;
        CustomerId = customerId;
        OrderDate = orderDate;
        TotalAmount = totalAmount;
    }
}