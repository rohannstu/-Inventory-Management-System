using FluentAssertions;
using InventoryManagementSystem.Application.Abstractions.Identity;
using InventoryManagementSystem.Application.Auth.Commands.Login;
using Moq;
using Xunit;

namespace InventoryManagementSystem.Tests.Auth;

public class LoginCommandHandlerTests
{
    [Fact]
    public async Task Should_Throw_UnauthorizedAccessException_When_Credentials_Are_Invalid()
    {
        var identityService = new Mock<IIdentityService>();
        identityService.Setup(x => x.ValidateCredentialsAsync("bad@email.com", "wrongpass", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidateCredentialsResult(false, null, Array.Empty<string>()));

        var tokenService = new Mock<ITokenService>();
        var handler = new LoginCommandHandler(identityService.Object, tokenService.Object);
        var command = new LoginCommand("bad@email.com", "wrongpass");

        await FluentActions.Invoking(() => handler.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Invalid email or password.");

        tokenService.Verify(x => x.GenerateTokens(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<IList<string>>()), Times.Never);
    }

    [Fact]
    public async Task Should_Return_LoginResult_When_Credentials_Are_Valid()
    {
        var userId = Guid.NewGuid();
        var identityService = new Mock<IIdentityService>();
        identityService.Setup(x => x.ValidateCredentialsAsync("test@example.com", "Password123", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidateCredentialsResult(true, userId, new List<string> { "Staff" }));

        var tokenService = new Mock<ITokenService>();
        tokenService.Setup(x => x.GenerateTokens(userId, "test@example.com", It.IsAny<IList<string>>()))
            .Returns(new TokenResult("access-token", "refresh-token", DateTime.UtcNow.AddHours(1)));

        var handler = new LoginCommandHandler(identityService.Object, tokenService.Object);
        var command = new LoginCommand("test@example.com", "Password123");

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.AccessToken.Should().Be("access-token");
        result.RefreshToken.Should().Be("refresh-token");
        result.ExpiresAtUtc.Should().BeCloseTo(DateTime.UtcNow.AddHours(1), TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task Should_Throw_UnauthorizedAccessException_When_UserId_Is_Null()
    {
        var identityService = new Mock<IIdentityService>();
        identityService.Setup(x => x.ValidateCredentialsAsync("test@example.com", "Password123", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidateCredentialsResult(false, null, Array.Empty<string>()));

        var tokenService = new Mock<ITokenService>();
        var handler = new LoginCommandHandler(identityService.Object, tokenService.Object);
        var command = new LoginCommand("test@example.com", "Password123");

        await FluentActions.Invoking(() => handler.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Invalid email or password.");
    }
}