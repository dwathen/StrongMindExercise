using StrongMindExercise.Application.Errors;
using StrongMindExercise.Application.Toppings.DTOs;
using StrongMindExercise.Domain.Toppings;

namespace StrongMindExercise.Application.Toppings.Services;
public class ToppingService
{
    private readonly IToppingRepository _toppingRepository;

    public ToppingService(IToppingRepository toppingRepository)
    {
        this._toppingRepository = toppingRepository;
    }

    public async Task<IEnumerable<ToppingReadDTO>> GetAllToppingsAsync() =>
        (await _toppingRepository.GetAllAsync())
    .Select(t => MapToDTO(t));

    public async Task<Result<ToppingReadDTO>> CreateToppingAsync(ToppingCreateDTO toppingCreateDTO)
    {
        var result = await ValidateEntry(toppingCreateDTO.Name);

        if (result.IsFailure)
            return Result<ToppingReadDTO>.Failure(result.Error);

        var topping = new Topping()
        {
            Name = toppingCreateDTO.Name
        };

        await _toppingRepository.AddAsync(topping);
        return Result<ToppingReadDTO>.Success(MapToDTO(topping));
    }

    public async Task<Result> UpdateToppingAsync(ToppingUpdateDTO toppingUpdateTO)
    {
        var result = await ValidateEntry(toppingUpdateTO.Name);

        if (result.IsFailure)
            return result;

        var topping = await _toppingRepository.GetByIdAsync(toppingUpdateTO.Id);

        if (topping is null)
            return Result.Failure(CommonErrors.ObjectCannotBeFound("Topping"));

        topping.Name = toppingUpdateTO.Name;
        await _toppingRepository.UpdateAsync(topping);
        return Result.Success();
    }

    public async Task<Result> DeleteToppingAsync(int id)
    {
        var topping = await _toppingRepository.GetByIdAsync(id);

        if (topping is null)
            return Result.Failure(CommonErrors.ObjectCannotBeFound("Topping"));

        await _toppingRepository.DeleteAsync(topping);

        return Result.Success();
    }

    private ToppingReadDTO MapToDTO(Topping topping)
    {
        return new ToppingReadDTO
        {
            Id = topping.Id,
            Name = topping.Name
        };
    }

    private async Task<Result> ValidateEntry(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(CommonErrors.NameCannotBeNullOrBlank);

        var existing = await _toppingRepository.GetByNameAsync(name);

        if (existing != null)
            return Result.Failure(CommonErrors.NameCannotBeDuplicate);

        return Result.Success();
    }
}
