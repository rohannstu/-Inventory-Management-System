using FluentAssertions;
using FluentValidation;
using InventoryManagementSystem.Application.Abstractions.Messaging;
using Moq;
using Xunit;

namespace InventoryManagementSystem.Tests.Pipeline;

public class ValidationBehaviorTests
{
    [Fact]
    public async Task Should_Call_Next_When_No_Validators()
    {
        var validators = new List<IValidator<TestRequest>>();
        var behavior = new ValidationBehavior<TestRequest, TestResponse>(validators);

        var nextCalled = false;
        RequestHandlerDelegate<TestResponse> next = () =>
        {
            nextCalled = true;
            return Task.FromResult(new TestResponse());
        };

        var result = await behavior.Handle(new TestRequest(), next, CancellationToken.None);

        nextCalled.Should().BeTrue();
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Should_Call_Next_When_All_Validators_Pass()
    {
        var validator = new TestPassValidator();
        var behavior = new ValidationBehavior<TestRequest, TestResponse>(new[] { validator });

        var nextCalled = false;
        RequestHandlerDelegate<TestResponse> next = () =>
        {
            nextCalled = true;
            return Task.FromResult(new TestResponse());
        };

        var result = await behavior.Handle(new TestRequest(), next, CancellationToken.None);

        nextCalled.Should().BeTrue();
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Should_Throw_ValidationException_When_Validator_Fails()
    {
        var validator = new TestFailValidator();
        var behavior = new ValidationBehavior<TestRequest, TestResponse>(new[] { validator });

        RequestHandlerDelegate<TestResponse> next = () => Task.FromResult(new TestResponse());

        await FluentActions.Invoking(() => behavior.Handle(new TestRequest(), next, CancellationToken.None))
            .Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task Should_Collect_Errors_From_Multiple_Validators()
    {
        var validator1 = new TestFailValidator1();
        var validator2 = new TestFailValidator2();
        var behavior = new ValidationBehavior<TestRequest, TestResponse>(new IValidator<TestRequest>[] { validator1, validator2 });

        RequestHandlerDelegate<TestResponse> next = () => Task.FromResult(new TestResponse());

        await FluentActions.Invoking(() => behavior.Handle(new TestRequest(), next, CancellationToken.None))
            .Should().ThrowAsync<ValidationException>()
            .Where(ex => ex.Errors.ToList().Count >= 2);
    }

    public sealed record TestRequest() : IRequest<TestResponse>;
    public sealed record TestResponse();

    private sealed class TestPassValidator : AbstractValidator<TestRequest>
    {
        public TestPassValidator()
        {
            RuleFor(x => x).NotNull();
        }
    }

    private sealed class TestFailValidator : AbstractValidator<TestRequest>
    {
        public TestFailValidator()
        {
            RuleFor(x => x).Must(_ => false).WithMessage("Error message");
        }
    }

    private sealed class TestFailValidator1 : AbstractValidator<TestRequest>
    {
        public TestFailValidator1()
        {
            RuleFor(x => x).Must(_ => false).WithMessage("Error 1");
        }
    }

    private sealed class TestFailValidator2 : AbstractValidator<TestRequest>
    {
        public TestFailValidator2()
        {
            RuleFor(x => x).Must(_ => false).WithMessage("Error 2");
        }
    }
}