using FluentAssertions;
using FluentValidation.TestHelper;
using InventoryManagementSystem.Application.Products.Commands.UpdateProduct;
using Xunit;

namespace InventoryManagementSystem.Tests.Products.Commands;

public class UpdateProductCommandValidatorTests
{
    [Fact]
    public void Should_Fail_When_Id_Is_Empty()
    {
        var validator = new UpdateProductCommandValidator();
        var result = validator.TestValidate(new UpdateProductCommand(
            Id: Guid.Empty,
            Name: "Test Product",
            Description: "Description",
            Price: 10.0m,
            Currency: "USD",
            CategoryId: Guid.NewGuid(),
            SupplierId: Guid.NewGuid()));

        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Fail_When_Name_Is_Empty()
    {
        var validator = new UpdateProductCommandValidator();
        var result = validator.TestValidate(new UpdateProductCommand(
            Id: Guid.NewGuid(),
            Name: string.Empty,
            Description: "Description",
            Price: 10.0m,
            Currency: "USD",
            CategoryId: Guid.NewGuid(),
            SupplierId: Guid.NewGuid()));

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Fail_When_Name_Exceeds_200_Characters()
    {
        var validator = new UpdateProductCommandValidator();
        var longName = new string('A', 201);
        var result = validator.TestValidate(new UpdateProductCommand(
            Id: Guid.NewGuid(),
            Name: longName,
            Description: "Description",
            Price: 10.0m,
            Currency: "USD",
            CategoryId: Guid.NewGuid(),
            SupplierId: Guid.NewGuid()));

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Fail_When_Price_Is_Negative()
    {
        var validator = new UpdateProductCommandValidator();
        var result = validator.TestValidate(new UpdateProductCommand(
            Id: Guid.NewGuid(),
            Name: "Test Product",
            Description: "Description",
            Price: -1.0m,
            Currency: "USD",
            CategoryId: Guid.NewGuid(),
            SupplierId: Guid.NewGuid()));

        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void Should_Fail_When_Currency_Is_Empty()
    {
        var validator = new UpdateProductCommandValidator();
        var result = validator.TestValidate(new UpdateProductCommand(
            Id: Guid.NewGuid(),
            Name: "Test Product",
            Description: "Description",
            Price: 10.0m,
            Currency: string.Empty,
            CategoryId: Guid.NewGuid(),
            SupplierId: Guid.NewGuid()));

        result.ShouldHaveValidationErrorFor(x => x.Currency);
    }

    [Fact]
    public void Should_Fail_When_Currency_Is_Not_3_Letters()
    {
        var validator = new UpdateProductCommandValidator();
        var result = validator.TestValidate(new UpdateProductCommand(
            Id: Guid.NewGuid(),
            Name: "Test Product",
            Description: "Description",
            Price: 10.0m,
            Currency: "US",
            CategoryId: Guid.NewGuid(),
            SupplierId: Guid.NewGuid()));

        result.ShouldHaveValidationErrorFor(x => x.Currency);
    }

    [Fact]
    public void Should_Fail_When_CategoryId_Is_Empty()
    {
        var validator = new UpdateProductCommandValidator();
        var result = validator.TestValidate(new UpdateProductCommand(
            Id: Guid.NewGuid(),
            Name: "Test Product",
            Description: "Description",
            Price: 10.0m,
            Currency: "USD",
            CategoryId: Guid.Empty,
            SupplierId: Guid.NewGuid()));

        result.ShouldHaveValidationErrorFor(x => x.CategoryId);
    }

    [Fact]
    public void Should_Fail_When_SupplierId_Is_Empty()
    {
        var validator = new UpdateProductCommandValidator();
        var result = validator.TestValidate(new UpdateProductCommand(
            Id: Guid.NewGuid(),
            Name: "Test Product",
            Description: "Description",
            Price: 10.0m,
            Currency: "USD",
            CategoryId: Guid.NewGuid(),
            SupplierId: Guid.Empty));

        result.ShouldHaveValidationErrorFor(x => x.SupplierId);
    }

    [Fact]
    public void Should_Pass_For_Valid_Command()
    {
        var validator = new UpdateProductCommandValidator();
        var result = validator.TestValidate(new UpdateProductCommand(
            Id: Guid.NewGuid(),
            Name: "Test Product",
            Description: "Description",
            Price: 10.0m,
            Currency: "USD",
            CategoryId: Guid.NewGuid(),
            SupplierId: Guid.NewGuid()));

        result.ShouldNotHaveAnyValidationErrors();
    }
}