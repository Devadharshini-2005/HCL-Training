
using FoodHub.DTOs.Category.Requests;
using FoodHub.Services;
using Microsoft.AspNetCore.Mvc;
using FoodHub.DTOs.Category.Requests;
using FoodHub.DTOs.Category.Responses;
using FoodHub.Models;

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
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryService.GetAllAsync();

        return Ok(categories);
    }

    // POST: api/categories
    [HttpPost]
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

    [HttpPut("{id:int}")]
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


}
