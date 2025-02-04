namespace StrongMindExercise.Application.Pizzas.DTOs;
public class PizzaUpdateDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<int> ToppingIds { get; set; }
}
