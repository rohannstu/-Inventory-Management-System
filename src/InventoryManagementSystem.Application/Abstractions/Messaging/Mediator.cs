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

        // We only know the handler's shape at runtime, so `dynamic` lets us call
        // Handle(...) on it without writing manual reflection/MethodInfo.Invoke code.
        return await ((dynamic)handler).Handle((dynamic)request, cancellationToken);
    }
}