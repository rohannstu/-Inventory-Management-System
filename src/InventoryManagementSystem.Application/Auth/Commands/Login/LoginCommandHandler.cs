using InventoryManagementSystem.Application.Abstractions.Identity;
using InventoryManagementSystem.Application.Abstractions.Messaging;
using InventoryManagementSystem.Application.Auth.Commands.Login;

namespace InventoryManagementSystem.Application.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
{
    private readonly IIdentityService _identityService;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(IIdentityService identityService, ITokenService tokenService)
    {
        _identityService = identityService;
        _tokenService = tokenService;
    }

    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var validation = await _identityService.ValidateCredentialsAsync(request.Email, request.Password, cancellationToken);

        if (!validation.Succeeded || validation.UserId is null)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var tokenResult = _tokenService.GenerateTokens(validation.UserId.Value, request.Email, validation.Roles);
        return new LoginResult(tokenResult.AccessToken, tokenResult.RefreshToken, tokenResult.AccessTokenExpiresAtUtc);
    }
}
