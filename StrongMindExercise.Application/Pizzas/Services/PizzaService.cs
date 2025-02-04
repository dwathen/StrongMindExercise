using StrongMindExercise.Application.Errors;
using StrongMindExercise.Application.Pizzas.DTOs;
using StrongMindExercise.Application.Toppings.DTOs;
using StrongMindExercise.Domain.Pizzas;
using StrongMindExercise.Domain.Toppings;

namespace StrongMindExercise.Application.Pizzas.Services;
public class PizzaService
{
    private readonly IPizzaRepository _pizzaRepository;
    private readonly IToppingRepository _toppingRepository;

    public PizzaService(IPizzaRepository pizzaRepository, IToppingRepository toppingRepository)
    {
        this._pizzaRepository = pizzaRepository;
        this._toppingRepository = toppingRepository;
    }

    public async Task<IEnumerable<PizzaReadDTO>> GetAllPizzasAsync() =>
        (await _pizzaRepository.GetAllAsync())
    .Select(t => MapToDTO(t));

    public async Task<Result<PizzaReadDTO>> CreatePizzaAsync(PizzaCreateDTO pizzaCreateDTO)
    {
        var result = await ValidateEntry(pizzaCreateDTO.Name, pizzaCreateDTO.ToppingIds);

        if (result.IsFailure)
            return Result<PizzaReadDTO>.Failure(result.Error);

        var toppingResult = await GetToppingsFromIds(pizzaCreateDTO.ToppingIds);

        if (toppingResult.IsFailure)
            return Result<PizzaReadDTO>.Failure(toppingResult.Error);

        var pizza = new Pizza()
        {
            Name = pizzaCreateDTO.Name,
            Toppings = toppingResult.Data
        };

        await _pizzaRepository.AddAsync(pizza);
        return Result<PizzaReadDTO>.Success(MapToDTO(pizza));
    }

    public async Task<Result> UpdatePizzaAsync(PizzaUpdateDTO pizzaUpdateDTO)
    {
        var result = await ValidateEntry(pizzaUpdateDTO.Name, pizzaUpdateDTO.ToppingIds, pizzaUpdateDTO.Id);

        if (result.IsFailure)
            return result;

        var pizza = await _pizzaRepository.GetByIdAsync(pizzaUpdateDTO.Id);

        if (pizza is null)
            return Result.Failure(CommonErrors.ObjectCannotBeFound("Pizza"));

        var toppingResult = await GetToppingsFromIds(pizzaUpdateDTO.ToppingIds);

        if (toppingResult.IsFailure)
            return Result<PizzaReadDTO>.Failure(toppingResult.Error);

        pizza.Name = pizzaUpdateDTO.Name;
        pizza.Toppings = toppingResult.Data;
        await _pizzaRepository.UpdateAsync(pizza);
        return Result.Success();
    }

    public async Task<Result> DeletePizzaAsync(int id)
    {
        var pizza = await _pizzaRepository.GetByIdAsync(id);

        if (pizza is null)
            return Result.Failure(CommonErrors.ObjectCannotBeFound("Pizza"));

        await _pizzaRepository.DeleteAsync(pizza);

        return Result.Success();
    }

    private PizzaReadDTO MapToDTO(Pizza pizza)
    {
        return new PizzaReadDTO
        {
            Id = pizza.Id,
            Name = pizza.Name,
            Toppings = pizza.Toppings.Select(MapToDTO).ToList()
        };
    }

    private ToppingReadDTO MapToDTO(Topping topping)
    {
        return new ToppingReadDTO
        {
            Id = topping.Id,
            Name = topping.Name
        };
    }

    private async Task<Result> ValidateEntry(string name, List<int>? toppingIds = null, int? id = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(CommonErrors.NameCannotBeNullOrBlank);

        var existing = await _pizzaRepository.GetByNameAsync(name);

        if (existing != null && id == null)
            return Result.Failure(CommonErrors.NameCannotBeDuplicate);

        if (existing != null && id != null && existing.Id != id)
            return Result.Failure(CommonErrors.NameCannotBeDuplicate);

        var hasDuplicates = toppingIds is not null &&
            toppingIds.GroupBy(x => x).Where(g => g.Count() > 1).Select(y => y.Key).Any();

        if (hasDuplicates)
            return Result.Failure(CommonErrors.CannotHaveDuplicateChildren);

        if (toppingIds is not null && toppingIds.Count == 0)
            return Result.Failure(CommonErrors.MustHaveAtLeastOneChild);

        return Result.Success();
    }

    private async Task<Result<List<Topping>>> GetToppingsFromIds(List<int> toppingIds)
    {
        List<Topping> toppings = new();

        foreach (var id in toppingIds)
        {
            var topping = await _toppingRepository.GetByIdAsync(id);

            if (topping is null)
            {
                return Result<List<Topping>>.Failure(CommonErrors.ObjectCannotBeFound("Topping"));
            }

            toppings.Add(await _toppingRepository.GetByIdAsync(id));
        }

        return Result<List<Topping>>.Success(toppings);
    }
}
