﻿using Microsoft.Extensions.DependencyInjection;

namespace StrongMindExercise.Persistence;
public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
    {
        return services;
    }
}
