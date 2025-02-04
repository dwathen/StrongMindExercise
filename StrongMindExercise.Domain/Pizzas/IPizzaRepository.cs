namespace StrongMindExercise.Domain.Pizzas;
public interface IPizzaRepository
{
    public Task<IEnumerable<Pizza>> GetAllAsync();
    public Task<Pizza> GetByIdAsync(int id);
    public Task<Pizza> GetByNameAsync(string name);
    public Task AddAsync(Pizza pizza);
    public Task UpdateAsync(Pizza pizza);
    public Task DeleteAsync(Pizza pizza);
}
