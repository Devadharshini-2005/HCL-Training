using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductManagementMVC.Models;
using ProductManagementMVC.Services.Interfaces;
using ProductManagementMVC.ViewModels;

namespace ProductManagementMVC.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public ProductsController(
        IProductService productService,
        ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
    int? categoryId,
    string? searchTerm,
    string sortOrder = "name_asc",
    int pageNumber = 1,
    int pageSize = 10)
    {
        pageNumber = Math.Max(pageNumber, 1);

        pageSize = pageSize is 10 or 20 or 50 ? pageSize : 10;

        ProductListViewModel model = await _productService.GetProductsAsync(
            searchTerm, categoryId, sortOrder, pageNumber, pageSize);

        model.Categories = await _categoryService.GetAllAsync();

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        Product? product = await _productService.GetByIdAsync(id);

        if (product is null)
        {
            return NotFound();
        }

        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadCategoriesAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product)
    {
        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync();
            return View(product);
        }

        bool created = await _productService.CreateAsync(product);

        if (!created)
        {
            ModelState.AddModelError(
                nameof(product.CategoryId),
                "The selected category does not exist.");

            await LoadCategoriesAsync();
            return View(product);
        }

        TempData["SuccessMessage"] = "Product created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        Product? product = await _productService.GetByIdAsync(id);

        if (product is null)
        {
            return NotFound();
        }

        await LoadCategoriesAsync();
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Product product)
    {
        if (id != product.ProductId)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync();
            return View(product);
        }

        bool updated = await _productService.UpdateAsync(id, product);

        if (!updated)
        {
            return NotFound();
        }

        TempData["SuccessMessage"] = "Product updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        Product? product = await _productService.GetByIdAsync(id);

        if (product is null)
        {
            return NotFound();
        }

        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        bool deleted = await _productService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        TempData["SuccessMessage"] = "Product deleted successfully.";
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadCategoriesAsync()
    {
        var categories = await _categoryService.GetAllAsync();

        ViewBag.Categories = new SelectList(
            categories,
            "CategoryId",
            "CategoryName");
    }
}
