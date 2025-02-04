using StrongMindExercise.Domain.Pizzas;

namespace StrongMindExercise.Domain.Toppings;
public class Topping
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Pizza> Pizzas { get; set; }
}
