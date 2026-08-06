using FluentAssertions;
using InventoryManagementSystem.Application.Abstractions.Persistence;
using InventoryManagementSystem.Application.Products.Commands.UpdateProduct;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.ValueObjects;
using Moq;
using Xunit;

namespace InventoryManagementSystem.Tests.Products.Commands;

public class UpdateProductCommandHandlerTests
{
    [Fact]
    public async Task Should_Return_False_When_Product_Not_Found()
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
    public async Task Should_Return_True_And_Update_Product_When_Product_Exists()
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

    [Fact]
    public async Task Should_Update_Only_Provided_Fields()
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
            Description: null,
            Price: 10.0m,
            Currency: "USD",
            CategoryId: existingProduct.CategoryId,
            SupplierId: existingProduct.SupplierId);

        await handler.Handle(request, CancellationToken.None);

        existingProduct.Name.Should().Be("Updated Product");
        existingProduct.Description.Should().BeNull();
    }
}