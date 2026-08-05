using InventoryManagementSystem.Application.Abstractions.Messaging;
using InventoryManagementSystem.Application.Abstractions.Identity;

namespace InventoryManagementSystem.Application.Auth.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : ICommand<LoginResult>;
