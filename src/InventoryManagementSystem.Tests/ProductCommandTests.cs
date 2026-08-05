using FluentAssertions;
using FluentValidation.TestHelper;
using InventoryManagementSystem.Application.Abstractions.Persistence;
using InventoryManagementSystem.Application.Products.Commands.CreateProduct;
using InventoryManagementSystem.Application.Products.Commands.UpdateProduct;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.ValueObjects;
using Moq;
using Xunit;

namespace InventoryManagementSystem.Tests;

public class ProductCommandTests
{
    [Fact]
    public void CreateProductCommandValidator_Should_Fail_When_Sku_Is_Empty()
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
    public void CreateProductCommandValidator_Should_Pass_For_Valid_Command()
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

    [Fact]
    public void UpdateProductCommandValidator_Should_Fail_When_Id_Is_Empty()
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
    public void UpdateProductCommandValidator_Should_Pass_For_Valid_Command()
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

    [Fact]
    public async Task CreateProductCommandHandler_Should_Create_Product_When_Sku_Is_Unique()
    {
        var repository = new Mock<IProductRepository>();
        repository.Setup(x => x.SkuExistsAsync("TESTSKU", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new CreateProductCommandHandler(repository.Object, unitOfWork.Object);
        var request = new CreateProductCommand(
            Sku: "TESTSKU",
            Name: "Test Product",
            Description: "Description",
            Price: 10.0m,
            Currency: "USD",
            CategoryId: Guid.NewGuid(),
            SupplierId: Guid.NewGuid());

        var productId = await handler.Handle(request, CancellationToken.None);

        productId.Should().NotBeEmpty();
        repository.Verify(x => x.AddAsync(It.Is<Product>(p => p.Sku.Value == "TESTSKU" && p.Name == "Test Product"), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateProductCommandHandler_Should_Throw_When_Sku_Already_Exists()
    {
        var repository = new Mock<IProductRepository>();
        repository.Setup(x => x.SkuExistsAsync("TESTSKU", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var unitOfWork = new Mock<IUnitOfWork>();
        var handler = new CreateProductCommandHandler(repository.Object, unitOfWork.Object);
        var request = new CreateProductCommand(
            Sku: "TESTSKU",
            Name: "Test Product",
            Description: "Description",
            Price: 10.0m,
            Currency: "USD",
            CategoryId: Guid.NewGuid(),
            SupplierId: Guid.NewGuid());

        await FluentActions.Invoking(() => handler.Handle(request, CancellationToken.None))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("A product with SKU 'TESTSKU' already exists.");

        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateProductCommandHandler_Should_Return_False_When_Product_Not_Found()
    {
        var repository = new Mock<IProductRepository>();
        repository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        var unitOfWork = new Mock<IUnitOfWork>();
        var handler = new UpdateProductCommandHandler(repository.Object, unitOfWork.Object);
        var request = new UpdateProductCommand(
            Id: Guid.NewGuid(),
            Name: "Updated Product",
            Description: "Updated Description",
            Price: 15.0m,
            Currency: "USD",
            CategoryId: Guid.NewGuid(),
            SupplierId: Guid.NewGuid());

        var result = await handler.Handle(request, CancellationToken.None);

        result.Should().BeFalse();
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateProductCommandHandler_Should_Return_True_When_Product_Exists()
    {
        var existingProduct = new Product(
            id: Guid.NewGuid(),
            sku: Sku.Create("TESTSKU"),
            name: "Original Product",
            price: Money.Create(10.0m, "USD"),
            categoryId: Guid.NewGuid(),
            supplierId: Guid.NewGuid(),
            description: "Original Description");

        var repository = new Mock<IProductRepository>();
        repository.Setup(x => x.GetByIdAsync(existingProduct.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new UpdateProductCommandHandler(repository.Object, unitOfWork.Object);
        var request = new UpdateProductCommand(
            Id: existingProduct.Id,
            Name: "Updated Product",
            Description: "Updated Description",
            Price: 20.0m,
            Currency: "USD",
            CategoryId: Guid.NewGuid(),
            SupplierId: Guid.NewGuid());

        var result = await handler.Handle(request, CancellationToken.None);

        result.Should().BeTrue();
        existingProduct.Name.Should().Be("Updated Product");
        existingProduct.Description.Should().Be("Updated Description");
        existingProduct.Price.Amount.Should().Be(20.0m);
        existingProduct.CategoryId.Should().Be(request.CategoryId);
        existingProduct.SupplierId.Should().Be(request.SupplierId);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
