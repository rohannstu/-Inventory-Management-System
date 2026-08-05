namespace InventoryManagementSystem.Application.Abstractions.Identity;

public record CreateUserResult(bool Succeeded, Guid? UserId, IReadOnlyList<string> Errors);
public record ValidateCredentialsResult(bool Succeeded, Guid? UserId, IList<string> Roles);

public interface IIdentityService
{
    Task<CreateUserResult> CreateUserAsync(string email, string password, string fullName, string role, CancellationToken cancellationToken);
    Task<ValidateCredentialsResult> ValidateCredentialsAsync(string email, string password, CancellationToken cancellationToken);
}
