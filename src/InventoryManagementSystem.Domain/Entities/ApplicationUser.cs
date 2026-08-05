using Microsoft.AspNetCore.Identity;

namespace InventoryManagementSystem.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FullName { get; set; } = default!;
}
