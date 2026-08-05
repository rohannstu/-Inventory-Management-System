namespace InventoryManagementSystem.Application.Auth.Commands.Login;

public sealed record LoginResult(string AccessToken, string RefreshToken, DateTime ExpiresAtUtc);
