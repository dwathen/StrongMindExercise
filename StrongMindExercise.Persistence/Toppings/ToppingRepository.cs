using StrongMindExercise.Domain.Toppings;

namespace StrongMindExercise.Persistence.Toppings;
public class ToppingRepository : IToppingRepository
{
    public Task AddAsync(Topping topping)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Topping topping)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Topping>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Topping> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Topping> GetByNameAsync(string name)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Topping topping)
    {
        throw new NotImplementedException();
    }
}
