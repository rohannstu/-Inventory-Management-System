using FluentAssertions;
using InventoryManagementSystem.Domain.ValueObjects;
using Xunit;

namespace InventoryManagementSystem.Tests.Domain;

public class MoneyTests
{
    [Fact]
    public void Create_Should_Return_Money_When_Valid()
    {
        var money = Money.Create(10.5m, "USD");

        money.Amount.Should().Be(10.5m);
        money.Currency.Should().Be("USD");
    }

    [Fact]
    public void Create_Should_Uppercase_Currency_Code()
    {
        var money = Money.Create(10.0m, "usd");

        money.Currency.Should().Be("USD");
    }

    [Fact]
    public void Create_Should_Throw_When_Amount_Is_Negative()
    {
        Action act = () => Money.Create(-1.0m, "USD");

        act.Should().Throw<ArgumentException>().WithMessage("Amount cannot be negative.*");
    }

    [Fact]
    public void Create_Should_Throw_When_Currency_Is_Null()
    {
        Action act = () => Money.Create(10.0m, null!);

        act.Should().Throw<ArgumentException>().WithMessage("Currency must be a 3-letter ISO code.*");
    }

    [Fact]
    public void Create_Should_Throw_When_Currency_Is_Empty()
    {
        Action act = () => Money.Create(10.0m, string.Empty);

        act.Should().Throw<ArgumentException>().WithMessage("Currency must be a 3-letter ISO code.*");
    }

    [Fact]
    public void Create_Should_Throw_When_Currency_Is_Not_3_Letters()
    {
        Action act = () => Money.Create(10.0m, "US");

        act.Should().Throw<ArgumentException>().WithMessage("Currency must be a 3-letter ISO code.*");
    }

    [Fact]
    public void Zero_Should_Return_Zero_Amount()
    {
        var money = Money.Zero("USD");

        money.Amount.Should().Be(0m);
        money.Currency.Should().Be("USD");
    }

    [Fact]
    public void Add_Should_Return_Sum_Of_Two_Money_Values()
    {
        var money1 = Money.Create(10.0m, "USD");
        var money2 = Money.Create(5.5m, "USD");

        var result = money1.Add(money2);

        result.Amount.Should().Be(15.5m);
        result.Currency.Should().Be("USD");
    }

    [Fact]
    public void Add_Should_Throw_When_Currencies_Differ()
    {
        var money1 = Money.Create(10.0m, "USD");
        var money2 = Money.Create(5.0m, "EUR");

        Action act = () => money1.Add(money2);

        act.Should().Throw<InvalidOperationException>().WithMessage("Cannot operate on Money with different currencies:*");
    }

    [Fact]
    public void Subtract_Should_Return_Difference_Of_Two_Money_Values()
    {
        var money1 = Money.Create(10.0m, "USD");
        var money2 = Money.Create(3.5m, "USD");

        var result = money1.Subtract(money2);

        result.Amount.Should().Be(6.5m);
        result.Currency.Should().Be("USD");
    }

    [Fact]
    public void Subtract_Should_Throw_When_Result_Is_Negative()
    {
        var money1 = Money.Create(5.0m, "USD");
        var money2 = Money.Create(10.0m, "USD");

        Action act = () => money1.Subtract(money2);

        act.Should().Throw<InvalidOperationException>().WithMessage("Resulting amount cannot be negative.");
    }

    [Fact]
    public void Subtract_Should_Throw_When_Currencies_Differ()
    {
        var money1 = Money.Create(10.0m, "USD");
        var money2 = Money.Create(5.0m, "EUR");

        Action act = () => money1.Subtract(money2);

        act.Should().Throw<InvalidOperationException>().WithMessage("Cannot operate on Money with different currencies:*");
    }

    [Fact]
    public void ToString_Should_Format_Amount_And_Currency()
    {
        var money = Money.Create(10.5m, "USD");

        money.ToString().Should().Be("10.50 USD");
    }
}