using InventoryManagementSystem.Application.Abstractions.Messaging;

namespace InventoryManagementSystem.Application.Auth.Commands.Register;

public record RegisterCommand(string Email, string Password, string FullName, string Role) : ICommand<Guid>;
