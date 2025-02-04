using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StrongMindExercise.Domain.Pizzas;
using StrongMindExercise.Domain.Toppings;
using StrongMindExercise.Persistence.Contexts;
using StrongMindExercise.Persistence.Pizzas;
using StrongMindExercise.Persistence.Toppings;

namespace StrongMindExercise.Persistence;
public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<StrongMindExerciseDbContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IToppingRepository, ToppingRepository>();
        services.AddScoped<IPizzaRepository, PizzaRepository>();

        return services;
    }
}
