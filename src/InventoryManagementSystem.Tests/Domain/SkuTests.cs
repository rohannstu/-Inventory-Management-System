using FluentAssertions;
using InventoryManagementSystem.Domain.ValueObjects;
using Xunit;

namespace InventoryManagementSystem.Tests.Domain;

public class SkuTests
{
    [Fact]
    public void Create_Should_Return_Sku_When_Value_Is_Valid()
    {
        var sku = Sku.Create("TESTSKU");

        sku.Value.Should().Be("TESTSKU");
    }

    [Fact]
    public void Create_Should_Trim_And_Uppercase_Value()
    {
        var sku = Sku.Create("  test-sku  ");

        sku.Value.Should().Be("TEST-SKU");
    }

    [Fact]
    public void Create_Should_Throw_When_Value_Is_Empty()
    {
        Action act = () => Sku.Create(string.Empty);

        act.Should().Throw<ArgumentException>().WithMessage("SKU cannot be empty.*");
    }

    [Fact]
    public void Create_Should_Throw_When_Value_Is_Null()
    {
        Action act = () => Sku.Create(null!);

        act.Should().Throw<ArgumentException>().WithMessage("SKU cannot be empty.*");
    }

    [Fact]
    public void Create_Should_Throw_When_Value_Exceeds_32_Characters()
    {
        Action act = () => Sku.Create("ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567");

        act.Should().Throw<ArgumentException>().WithMessage("SKU cannot exceed 32 characters.*");
    }

    [Fact]
    public void Create_Should_Throw_When_Value_Contains_Invalid_Characters()
    {
        Action act = () => Sku.Create("TEST@SKU");

        act.Should().Throw<ArgumentException>().WithMessage("SKU can only contain letters, digits, and hyphens.*");
    }

    [Fact]
    public void Create_Should_Allow_Letters_Digits_And_Hyphens()
    {
        var sku = Sku.Create("ABC-123-XYZ");

        sku.Value.Should().Be("ABC-123-XYZ");
    }

    [Fact]
    public void ToString_Should_Return_Value()
    {
        var sku = Sku.Create("TESTSKU");

        sku.ToString().Should().Be("TESTSKU");
    }
}