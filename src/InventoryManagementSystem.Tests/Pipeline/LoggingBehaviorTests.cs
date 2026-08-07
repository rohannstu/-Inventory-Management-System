using FluentAssertions;
using InventoryManagementSystem.Application.Abstractions.Messaging;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace InventoryManagementSystem.Tests.Pipeline;

public class LoggingBehaviorTests
{
    [Fact]
    public async Task Should_Log_Handling_And_Handled_Messages()
    {
        var logger = new Mock<ILogger<LoggingBehavior<TestRequest, TestResponse>>>();
        var behavior = new LoggingBehavior<TestRequest, TestResponse>(logger.Object);

        RequestHandlerDelegate<TestResponse> next = () => Task.FromResult(new TestResponse());

        var result = await behavior.Handle(new TestRequest(), next, CancellationToken.None);

        result.Should().NotBeNull();
        logger.Verify(x => x.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Handling")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
        logger.Verify(x => x.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Handled")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [Fact]
    public async Task Should_Call_Next_And_Return_Response()
    {
        var logger = new Mock<ILogger<LoggingBehavior<TestRequest, TestResponse>>>();
        var behavior = new LoggingBehavior<TestRequest, TestResponse>(logger.Object);

        var expectedResponse = new TestResponse();
        RequestHandlerDelegate<TestResponse> next = () => Task.FromResult(expectedResponse);

        var result = await behavior.Handle(new TestRequest(), next, CancellationToken.None);

        result.Should().BeSameAs(expectedResponse);
    }

    public sealed record TestRequest() : IRequest<TestResponse>;
    public sealed record TestResponse();
}