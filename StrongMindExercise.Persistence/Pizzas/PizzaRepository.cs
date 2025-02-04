using StrongMindExercise.Domain.Pizzas;

namespace StrongMindExercise.Persistence.Pizzas;
public class PizzaRepository : IPizzaRepository
{
    public Task AddAsync(Pizza pizza)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Pizza pizza)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Pizza>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Pizza> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Pizza> GetByNameAsync(string name)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Pizza pizza)
    {
        throw new NotImplementedException();
    }
}
