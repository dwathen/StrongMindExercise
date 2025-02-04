using Microsoft.AspNetCore.Mvc;
using StrongMindExercise.API.ControllerResult;
using StrongMindExercise.Application.Pizzas.DTOs;
using StrongMindExercise.Application.Pizzas.Services;

namespace StrongMindExercise.API.Pizzas;
[Route("api/[controller]")]
[ApiController]
public class PizzaController : ControllerResultBase
{
    private readonly PizzaService _pizzaService;

    public PizzaController(PizzaService pizzaService)
    {
        this._pizzaService = pizzaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PizzaReadDTO>>> GetToppings()
    {
        try
        {
            var toppings = await _pizzaService.GetAllPizzasAsync();
            return Ok(toppings);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<PizzaReadDTO>> CreateTopping([FromBody] PizzaCreateDTO pizzaCreateDTO)
    {
        try
        {
            var result = await _pizzaService.CreatePizzaAsync(pizzaCreateDTO);

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
    public async Task<IActionResult> UpdateTopping([FromBody] PizzaUpdateDTO pizzaUpdateDTO)
    {
        try
        {
            var result = await _pizzaService.UpdatePizzaAsync(pizzaUpdateDTO);

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
            var result = await _pizzaService.DeletePizzaAsync(id);

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
