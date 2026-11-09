using ProductManagementMVC.Models;

namespace ProductManagementMVC.ViewModels;

public class ProductListViewModel
{
    public IReadOnlyList<Product> Products { get; set; } = [];
    public IReadOnlyList<Category> Categories { get; set; } = [];

    public string? SearchTerm { get; set; }
    public int? CategoryId { get; set; }
    public string SortOrder { get; set; } = "name_asc";

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalRecords { get; set; }

    public int TotalPages =>
        (int)Math.Ceiling(TotalRecords / (double)PageSize);
}
