namespace ProductManagementMVC.ViewModels;

public class DashboardViewModel
{
    public int TotalProducts { get; set; }
    public int TotalCategories { get; set; }
    public decimal InventoryValue { get; set; }
    public decimal AveragePrice { get; set; }
    public string? MostExpensiveProduct { get; set; }
    public IReadOnlyList<CategoryProductCountViewModel> CategoryCounts { get; set; } = [];
}

public class CategoryProductCountViewModel
{
    public string CategoryName { get; set; } = string.Empty;
    public int ProductCount { get; set; }
}
