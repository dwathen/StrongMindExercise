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

    public async Task<Result<List<ToppingReadDTO>>> GetAllToppingsAsync()
    {
        var toppings = (await _toppingRepository.GetAllAsync())
            .Select(t => MapToDTO(t)).ToList();

        return Result<List<ToppingReadDTO>>.Success(toppings);
    }
        

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

    public async Task<Result> UpdateToppingAsync(ToppingUpdateDTO toppingUpdateDTO)
    {
        var result = await ValidateEntry(toppingUpdateDTO.Name, toppingUpdateDTO.Id);

        if (result.IsFailure)
            return result;

        var topping = await _toppingRepository.GetByIdAsync(toppingUpdateDTO.Id);

        if (topping is null)
            return Result.Failure(CommonErrors.ObjectCannotBeFound("Topping"));

        topping.Name = toppingUpdateDTO.Name;
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

    private async Task<Result> ValidateEntry(string name, int? id = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(CommonErrors.NameCannotBeNullOrBlank);

        var existing = await _toppingRepository.GetByNameAsync(name);

        if (existing != null && id == null)
            return Result.Failure(CommonErrors.NameCannotBeDuplicate);

        if (existing != null && id != null && existing.Id != id)
            return Result.Failure(CommonErrors.NameCannotBeDuplicate);

        return Result.Success();
    }
}
