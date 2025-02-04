using StrongMindExercise.Domain.Toppings;

namespace StrongMindExercise.Domain.Pizzas;
public class Pizza
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Topping> Toppings { get; set; }
}
