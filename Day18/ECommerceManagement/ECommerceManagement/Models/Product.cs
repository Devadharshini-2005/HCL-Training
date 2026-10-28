namespace ECommerceManagement.Models;

public class Product
{
    public int ProductId { get; set; }
    public string? ProductName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int CategoryId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public Product()
    {
        
    }
    public Product(string productName, string? description, decimal price, int stockQuantity, int categoryId)
    {
        ProductId++;
        ProductName = productName;
        Description = description;
        Price = price;
        StockQuantity = stockQuantity;
        CategoryId = categoryId;
        IsActive = true; 
        CreatedAt = DateTime.Now;
    }
}