using StrongMindExercise.Application.Toppings.DTOs;

namespace StrongMindExercise.Application.Pizzas.DTOs;
public class PizzaReadDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<ToppingReadDTO> Toppings { get; set; }
}
