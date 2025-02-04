using Microsoft.Extensions.DependencyInjection;
using StrongMindExercise.Application.Pizzas.Services;
using StrongMindExercise.Application.Toppings.Services;

namespace StrongMindExercise.Application;
public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ToppingService>();
        services.AddScoped<PizzaService>();

        return services;
    }
}
