namespace StrongMindExercise.Domain.Toppings;
public interface IToppingRepository
{
    public Task<IEnumerable<Topping>?> GetAllAsync();
    public Task<Topping?> GetByIdAsync(int id);
    public Task<Topping?> GetByNameAsync(string name);
    public Task AddAsync(Topping topping);
    public Task UpdateAsync(Topping topping);
    public Task DeleteAsync(Topping topping);
}
