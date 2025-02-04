using Microsoft.EntityFrameworkCore;
using StrongMindExercise.Domain.Pizzas;
using StrongMindExercise.Domain.Toppings;

namespace StrongMindExercise.Persistence.Contexts;
public class StrongMindExerciseDbContext : DbContext
{
    public StrongMindExerciseDbContext(DbContextOptions<StrongMindExerciseDbContext> options) : base(options) { }

    public DbSet<Topping> Toppings { get; set; }
    public DbSet<Pizza> Pizzas { get; set; }
}
