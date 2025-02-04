using Microsoft.EntityFrameworkCore;
using StrongMindExercise.Domain.Pizzas;
using StrongMindExercise.Persistence.Contexts;

namespace StrongMindExercise.Persistence.Pizzas;
public class PizzaRepository : IPizzaRepository
{
    private readonly StrongMindExerciseDbContext _context;

    public PizzaRepository(StrongMindExerciseDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Pizza>> GetAllAsync() =>
        await _context.Pizzas.Include(p => p.Toppings).ToListAsync();

    public async Task<Pizza> GetByIdAsync(int id) =>
        await _context.Pizzas.Include(p => p.Toppings).FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Pizza> GetByNameAsync(string name) =>
        await _context.Pizzas.Include(p => p.Toppings)
            .FirstOrDefaultAsync(p => p.Name.ToUpper() == name.ToUpper());

    public async Task AddAsync(Pizza pizza)
    {
        _context.Pizzas.Add(pizza);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Pizza pizza)
    {
        _context.Pizzas.Update(pizza);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Pizza pizza)
    {
        _context.Pizzas.Remove(pizza);
        await _context.SaveChangesAsync();
    }
}
