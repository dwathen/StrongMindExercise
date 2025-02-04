using Microsoft.EntityFrameworkCore;
using StrongMindExercise.Domain.Toppings;
using StrongMindExercise.Persistence.Contexts;

namespace StrongMindExercise.Persistence.Toppings;
public class ToppingRepository : IToppingRepository
{
    private readonly StrongMindExerciseDbContext _context;

    public ToppingRepository(StrongMindExerciseDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Topping>> GetAllAsync() =>
        await _context.Toppings.ToListAsync();

    public async Task<Topping> GetByIdAsync(int id) =>
        await _context.Toppings.FindAsync(id);

    public async Task<Topping> GetByNameAsync(string name) =>
        await _context.Toppings.FirstOrDefaultAsync(t => t.Name.ToUpper() == name.ToUpper());

    public async Task AddAsync(Topping topping)
    {
        _context.Toppings.Add(topping);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Topping topping)
    {
        _context.Toppings.Update(topping);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Topping topping)
    {
        _context.Toppings.Remove(topping);
        await _context.SaveChangesAsync();
    }
}
