using Microsoft.AspNetCore.Mvc;
using StrongMindExercise.API.ControllerResult;
using StrongMindExercise.Application.Toppings.DTOs;
using StrongMindExercise.Application.Toppings.Services;

namespace StrongMindExercise.API.Toppings;
[Route("api/[controller]")]
[ApiController]
public class ToppingController : ControllerResultBase
{
    private readonly ToppingService _toppingService;

    public ToppingController(ToppingService toppingService)
    {
        _toppingService = toppingService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ToppingReadDTO>>> GetToppings()
    {
        try
        {
            var toppings = await _toppingService.GetAllToppingsAsync();
            return Ok(toppings);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<ToppingReadDTO>> CreateTopping([FromBody] ToppingCreateDTO toppingCreateDto)
    {
        try
        {
            var result = await _toppingService.CreateToppingAsync(toppingCreateDto);

            if (result.IsFailure)
            {
                return HandleResult(result);
            }

            return CreatedAtAction(nameof(GetToppings), new { result.Data.Name });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTopping([FromBody] ToppingUpdateDTO toppingUpdateDto)
    {
        try
        {
            var result = await _toppingService.UpdateToppingAsync(toppingUpdateDto);

            if (result.IsFailure)
            {
                return HandleResult(result);
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTopping(int id)
    {
        try
        {
            var result = await _toppingService.DeleteToppingAsync(id);

            if (result.IsFailure)
            {
                return HandleResult(result);
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
