namespace ECommerceManagement.Models;

public class CartItem
{
    public int CartItemId { get; set; }
    public int CustomerId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal LineTotal => UnitPrice * Quantity;
    public DateTime AddedAt { get; set; }
}