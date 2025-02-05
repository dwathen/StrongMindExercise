using StrongMindExercise.Application.Errors;
using StrongMindExercise.Application.Pizzas.DTOs;
using StrongMindExercise.WebUI.HelperMethods;

namespace StrongMindExercise.WebUI.Pizzas;

public class PizzaWebService
{
    private readonly string _baseUrl;

    public PizzaWebService(IConfiguration configuration)
    {
        this._baseUrl = configuration["APIUrl"] + "Pizza";
    }

    public async Task<List<PizzaReadDTO>> GetPizzasAsync()
    {
        return await HttpHelperMethods.GetAsync<List<PizzaReadDTO>>($"{_baseUrl}");
    }

    public async Task<Error> CreatePizzaAsync(PizzaCreateDTO pizzaCreateDTO)
    {
        return await HttpHelperMethods.PostAsync($"{_baseUrl}", pizzaCreateDTO);
    }

    public async Task<Error> UpdatePizzaAsync(PizzaUpdateDTO pizzaUpdateDTO)
    {
        return await HttpHelperMethods.PutAsync($"{_baseUrl}", pizzaUpdateDTO);
    }

    public async Task<Error> DeletePizzaAsync(int id)
    {
        return await HttpHelperMethods.DeleteAsync($"{_baseUrl}/{id}");
    }
}
