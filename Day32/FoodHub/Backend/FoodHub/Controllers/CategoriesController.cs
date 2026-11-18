
using FoodHub.DTOs.Category.Requests;
using FoodHub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodHub.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET: api/categories
    [HttpGet]
    [Authorize(Policy = "AdminOrCustomer")]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryService.GetAllAsync();

        return Ok(categories);
    }

    // POST: api/categories
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create(CreateCategoryDto request)
    {
        var category = await _categoryService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetAll),
            category
        );
    }

    // GET: api/categories/2
    [HttpGet("{id:int}")]
    [Authorize(Policy = "AdminOrCustomer")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);

        if (category == null)
        {
            return NotFound(new
            {
                message = $"Category with ID {id} was not found."
            });
        }

        return Ok(category);
    }

    // PUT: api/categories/2
    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Update(
        int id,
        UpdateCategoryDto request)
    {
        var category = await _categoryService.UpdateAsync(id, request);

        if (category == null)
        {
            return NotFound(new
            {
                message = $"Category with ID {id} was not found."
            });
        }

        return Ok(category);
    }

    // DELETE: api/categories/2
    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var isDeleted = await _categoryService.DeleteAsync(id);

            if (!isDeleted)
            {
                return NotFound(new
                {
                    message = $"Category with ID {id} was not found."
                });
            }

            return Ok(new
            {
                message = "Category deleted successfully."
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }
}
