using Microsoft.Extensions.DependencyInjection;

namespace InventoryManagementSystem.Application.Abstractions.Messaging;

public class Mediator(IServiceProvider serviceProvider) : IMediator
{
    public async Task<TResponse> Send<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();

        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
        var handler = serviceProvider.GetRequiredService(handlerType);

        // The innermost link in the chain: actually calling the real handler.
        RequestHandlerDelegate<TResponse> pipeline = () =>
            ((dynamic)handler).Handle((dynamic)request, cancellationToken);

        var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, typeof(TResponse));
        var behaviors = serviceProvider.GetServices(behaviorType).Reverse().ToList();

        // Wrap the pipeline in each behavior, working from innermost to outermost,
        // so the FIRST-registered behavior ends up running FIRST at execution time.
        foreach (var behavior in behaviors)
        {
            var next = pipeline;
            pipeline = () => ((dynamic)behavior).Handle((dynamic)request, next, cancellationToken);
        }

        return await pipeline();
    }
}