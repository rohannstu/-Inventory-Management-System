namespace InventoryManagementSystem.Application.Abstractions.Identity;

public record CreateUserResult(bool Succeeded, Guid? UserId, IReadOnlyList<string> Errors);

public interface IIdentityService
{
    Task<CreateUserResult> CreateUserAsync(string email, string password, string fullName, string role, CancellationToken cancellationToken);
}
