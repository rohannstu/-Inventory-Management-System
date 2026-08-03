namespace InventoryManagementSystem.Application.Abstractions.Messaging;

public record PingCommand : ICommand<string>;

public class PingCommandHandler : IRequestHandler<PingCommand, string>
{
    public Task<string> Handle(PingCommand request, CancellationToken cancellationToken)
        => Task.FromResult("pong");
}
