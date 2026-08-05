using InventoryManagementSystem.Application.Abstractions.Identity;
using InventoryManagementSystem.Application.Abstractions.Messaging;

namespace InventoryManagementSystem.Application.Auth.Commands.Register;

public class RegisterCommandHandler(IIdentityService identityService)
    : IRequestHandler<RegisterCommand, Guid>
{
    public async Task<Guid> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var result = await identityService.CreateUserAsync(
            request.Email, request.Password, request.FullName, request.Role, cancellationToken);

        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join("; ", result.Errors));

        return result.UserId!.Value;
    }
}
