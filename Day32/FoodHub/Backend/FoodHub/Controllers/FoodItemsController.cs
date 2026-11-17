
using FoodHub.DTOs.FoodItem.Requests;
using FoodHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodHub.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FoodItemsController : ControllerBase
{
    private readonly IFoodItemService _foodItemService;

    public FoodItemsController(IFoodItemService foodItemService)
    {
        _foodItemService = foodItemService;
    }

    // GET: api/fooditems
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var foodItems = await _foodItemService.GetAllAsync();

        return Ok(foodItems);
    }

    // GET: api/fooditems/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var foodItem = await _foodItemService.GetByIdAsync(id);

        if (foodItem == null)
        {
            return NotFound(new { message = "Food item not found." });
        }

        return Ok(foodItem);
    }

    // POST: api/fooditems
    [HttpPost]
    public async Task<IActionResult> Add(
        [FromBody] CreateFoodItemDto foodItemDto)
    {
        var createdFoodItem =
            await _foodItemService.AddAsync(foodItemDto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdFoodItem.FoodItemId },
            createdFoodItem);
    }

    // PUT: api/fooditems/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateFoodItemDto foodItemDto)
    {
        await _foodItemService.UpdateAsync(id, foodItemDto);

        return NoContent();
    }

    // DELETE: api/fooditems/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _foodItemService.DeleteAsync(id);

        return NoContent();
    }
}
