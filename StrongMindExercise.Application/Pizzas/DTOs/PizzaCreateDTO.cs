namespace StrongMindExercise.Application.Pizzas.DTOs;
public class PizzaCreateDTO
{
    public string Name { get; set; }
    public List<int> ToppingIds { get; set; }
}
