using Microsoft.Extensions.DependencyInjection;
using ProductManager.Services.Abstractions;

namespace ProductManager.Services;

/// <summary>
/// Extension methods for registering ProductManager services into an IoC container.
/// Usage: services.AddProductManagerServices()
/// 
/// Separating registration from the service classes themselves follows
/// the Open/Closed Principle — adding a new service never requires
/// touching existing code, only this file.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all ProductManager services.
    /// <see cref="WarehouseRepository"/> is registered as a singleton because
    /// FakeDataStore is a static in-memory store — one shared instance is correct.
    /// In a later lab with a real DB context this would likely become Scoped.
    /// </summary>
    public static IServiceCollection AddProductManagerServices(this IServiceCollection services)
    {
        services.AddSingleton<IWarehouseRepository, WarehouseRepository>();
        return services;
    }
}
