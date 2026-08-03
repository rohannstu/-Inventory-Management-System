using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryManagementSystem.Application.Abstractions.Messaging;

public static class MediatorServiceCollectionExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddScoped<IMediator, Mediator>();

        var handlerInterfaceType = typeof(IRequestHandler<,>);

        foreach (var assembly in assemblies)
        {
            var matches =
                from type in assembly.GetTypes()
                where !type.IsAbstract && !type.IsInterface
                from @interface in type.GetInterfaces()
                where @interface.IsGenericType
                      && @interface.GetGenericTypeDefinition() == handlerInterfaceType
                select new { Implementation = type, Interface = @interface };

            foreach (var match in matches)
            {
                services.AddScoped(match.Interface, match.Implementation);
            }
        }

        return services;
    }
}