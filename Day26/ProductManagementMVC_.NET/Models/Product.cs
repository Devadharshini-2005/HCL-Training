using System.ComponentModel.DataAnnotations;

namespace ProductManagementMVC.Models;

public class Product
{
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Product name is required.")]
    [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
    public string ProductName { get; set; } = string.Empty;

    [Range(typeof(decimal), "0.01", "100000000",
        ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
    public int CategoryId { get; set; }

    public Category? Category { get; set; }
}
