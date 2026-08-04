using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryManagementSystem.Application.Abstractions.Messaging;

public static class MediatorServiceCollectionExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddScoped<IMediator, Mediator>();
        // Register pipeline behaviors: Logging wraps outside Validation
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

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

        // Register FluentValidation validators found in the provided assemblies
        foreach (var assembly in assemblies)
        {
            var validatorMatches =
                from type in assembly.GetTypes()
                where !type.IsAbstract && !type.IsInterface
                from @interface in type.GetInterfaces()
                where @interface.IsGenericType
                      && @interface.GetGenericTypeDefinition() == typeof(IValidator<>)
                select new { Implementation = type, Interface = @interface };

            foreach (var match in validatorMatches)
            {
                services.AddScoped(match.Interface, match.Implementation);
            }
        }

        return services;
    }
}