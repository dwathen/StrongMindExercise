using StrongMindExercise.Application.Errors;
using StrongMindExercise.Application.Toppings.DTOs;
using StrongMindExercise.WebUI.HelperMethods;

namespace StrongMindExercise.WebUI.Toppings;

public class ToppingWebService
{
    private readonly string _baseUrl;

    public ToppingWebService(IConfiguration configuration)
    {
        this._baseUrl = configuration["APIUrl"] + "Topping";
    }

    public async Task<List<ToppingReadDTO>> GetToppingsAsync()
    {
        return await HttpHelperMethods.GetAsync<List<ToppingReadDTO>>($"{_baseUrl}");
    }

    public async Task<Error> CreateToppingAsync(ToppingCreateDTO toppingCreateDto)
    {
        return await HttpHelperMethods.PostAsync($"{_baseUrl}", toppingCreateDto);
    }

    public async Task<Error> UpdateToppingAsync(ToppingUpdateDTO toppingUpdateDto)
    {
        return await HttpHelperMethods.PutAsync($"{_baseUrl}", toppingUpdateDto);
    }

    public async Task<Error> DeleteToppingAsync(int id)
    {
        return await HttpHelperMethods.DeleteAsync($"{_baseUrl}/{id}");
    }
}
