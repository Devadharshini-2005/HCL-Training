using Microsoft.AspNetCore.Mvc;
using ProductManagementMVC.Models;
using ProductManagementMVC.Services.Interfaces;

namespace ProductManagementMVC.Controllers;

public class CategoriesController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View(await _categoryService.GetAllAsync());
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        Category? category = await _categoryService.GetByIdAsync(id);

        if (category is null)
        {
            return NotFound();
        }

        return View(category);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category category)
    {
        if (!ModelState.IsValid)
        {
            return View(category);
        }

        bool created = await _categoryService.CreateAsync(category);

        if (!created)
        {
            ModelState.AddModelError(
                nameof(category.CategoryName),
                "A category with this name already exists.");

            return View(category);
        }

        TempData["SuccessMessage"] = "Category created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        Category? category = await _categoryService.GetByIdAsync(id);

        if (category is null)
        {
            return NotFound();
        }

        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Category category)
    {
        if (id != category.CategoryId)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(category);
        }

        bool updated = await _categoryService.UpdateAsync(id, category);

        if (!updated)
        {
            ModelState.AddModelError(
                nameof(category.CategoryName),
                "Category not found or another category already uses this name.");

            return View(category);
        }

        TempData["SuccessMessage"] = "Category updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        Category? category = await _categoryService.GetByIdAsync(id);

        if (category is null)
        {
            return NotFound();
        }

        ViewBag.HasProducts = await _categoryService.HasProductsAsync(id);
        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        bool deleted = await _categoryService.DeleteAsync(id);

        if (!deleted)
        {
            TempData["ErrorMessage"] =
                "Category could not be deleted. It may contain products.";
            return RedirectToAction(nameof(Index));
        }

        TempData["SuccessMessage"] = "Category deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}
