using FluentAssertions;
using InventoryManagementSystem.Application.Abstractions.Persistence;
using InventoryManagementSystem.Application.Products.Commands.CreateProduct;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.ValueObjects;
using Moq;
using Xunit;

namespace InventoryManagementSystem.Tests.Products.Commands;

public class CreateProductCommandHandlerTests
{
    [Fact]
    public async Task Should_Create_Product_When_Sku_Is_Unique()
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
        repository.Verify(x => x.AddAsync(It.Is<Product>(p =>
            p.Sku.Value == "TESTSKU" && p.Name == "Test Product"), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Should_Throw_When_Sku_Already_Exists()
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
    public async Task Should_Set_Default_StockQuantity_To_Zero()
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

        await handler.Handle(request, CancellationToken.None);

        repository.Verify(x => x.AddAsync(It.Is<Product>(p => p.StockQuantity == 0), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Should_Set_IsActive_To_True()
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

        await handler.Handle(request, CancellationToken.None);

        repository.Verify(x => x.AddAsync(It.Is<Product>(p => p.IsActive == true), It.IsAny<CancellationToken>()), Times.Once);
    }
}