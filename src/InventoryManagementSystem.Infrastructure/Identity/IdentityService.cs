using InventoryManagementSystem.Application.Abstractions.Identity;
using InventoryManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace InventoryManagementSystem.Infrastructure.Identity;

public class IdentityService(UserManager<ApplicationUser> userManager) : IIdentityService
{
    public async Task<CreateUserResult> CreateUserAsync(
        string email, string password, string fullName, string role, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser { UserName = email, Email = email, FullName = fullName };
        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            return new CreateUserResult(false, null, result.Errors.Select(e => e.Description).ToList());

        await userManager.AddToRoleAsync(user, role);
        return new CreateUserResult(true, user.Id, Array.Empty<string>());
    }

    public async Task<ValidateCredentialsResult> ValidateCredentialsAsync(
        string email, string password, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null || !await userManager.CheckPasswordAsync(user, password))
            return new ValidateCredentialsResult(false, null, Array.Empty<string>());

        var roles = await userManager.GetRolesAsync(user);
        return new ValidateCredentialsResult(true, user.Id, roles);
    }
}
