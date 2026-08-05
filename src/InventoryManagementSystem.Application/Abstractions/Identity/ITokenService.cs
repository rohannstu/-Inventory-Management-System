namespace InventoryManagementSystem.Application.Abstractions.Identity;

public record TokenResult(string AccessToken, string RefreshToken, DateTime AccessTokenExpiresAtUtc);

public interface ITokenService
{
    TokenResult GenerateTokens(Guid userId, string email, IList<string> roles);
}
