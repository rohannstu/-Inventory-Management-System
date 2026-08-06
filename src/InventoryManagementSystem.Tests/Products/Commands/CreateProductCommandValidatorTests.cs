using FluentAssertions;
using FluentValidation.TestHelper;
using InventoryManagementSystem.Application.Products.Commands.CreateProduct;
using Xunit;

namespace InventoryManagementSystem.Tests.Products.Commands;

public class CreateProductCommandValidatorTests
{
    [Fact]
    public void Should_Fail_When_Sku_Is_Empty()
    {
        var validator = new CreateProductCommandValidator();
        var result = validator.TestValidate(new CreateProductCommand(
            Sku: string.Empty,
            Name: "Test Product",
            Description: "Description",
            Price: 10.0m,
            Currency: "USD",
            CategoryId: Guid.NewGuid(),
            SupplierId: Guid.NewGuid()));

        result.ShouldHaveValidationErrorFor(x => x.Sku);
    }

    [Fact]
    public void Should_Fail_When_Sku_Exceeds_32_Characters()
    {
        var validator = new CreateProductCommandValidator();
        var result = validator.TestValidate(new CreateProductCommand(
            Sku: "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567",
            Name: "Test Product",
            Description: "Description",
            Price: 10.0m,
            Currency: "USD",
            CategoryId: Guid.NewGuid(),
            SupplierId: Guid.NewGuid()));

        result.ShouldHaveValidationErrorFor(x => x.Sku);
    }

    [Fact]
    public void Should_Fail_When_Name_Is_Empty()
    {
        var validator = new CreateProductCommandValidator();
        var result = validator.TestValidate(new CreateProductCommand(
            Sku: "TESTSKU",
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
        var validator = new CreateProductCommandValidator();
        var longName = new string('A', 201);
        var result = validator.TestValidate(new CreateProductCommand(
            Sku: "TESTSKU",
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
        var validator = new CreateProductCommandValidator();
        var result = validator.TestValidate(new CreateProductCommand(
            Sku: "TESTSKU",
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
        var validator = new CreateProductCommandValidator();
        var result = validator.TestValidate(new CreateProductCommand(
            Sku: "TESTSKU",
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
        var validator = new CreateProductCommandValidator();
        var result = validator.TestValidate(new CreateProductCommand(
            Sku: "TESTSKU",
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
        var validator = new CreateProductCommandValidator();
        var result = validator.TestValidate(new CreateProductCommand(
            Sku: "TESTSKU",
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
        var validator = new CreateProductCommandValidator();
        var result = validator.TestValidate(new CreateProductCommand(
            Sku: "TESTSKU",
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
        var validator = new CreateProductCommandValidator();
        var result = validator.TestValidate(new CreateProductCommand(
            Sku: "TESTSKU",
            Name: "Test Product",
            Description: "Description",
            Price: 10.0m,
            Currency: "USD",
            CategoryId: Guid.NewGuid(),
            SupplierId: Guid.NewGuid()));

        result.ShouldNotHaveAnyValidationErrors();
    }
}