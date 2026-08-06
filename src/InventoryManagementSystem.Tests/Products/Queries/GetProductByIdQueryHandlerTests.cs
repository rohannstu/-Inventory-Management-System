using FluentAssertions;
using InventoryManagementSystem.Application.Abstractions.Persistence;
using InventoryManagementSystem.Application.Products.Queries.GetProductById;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.ValueObjects;
using Moq;
using Xunit;

namespace InventoryManagementSystem.Tests.Products.Queries;

public class GetProductByIdQueryHandlerTests
{
    [Fact]
    public async Task Should_Return_Null_When_Product_Not_Found()
    {
        var repository = new Mock<IProductRepository>();
        repository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        var handler = new GetProductByIdQueryHandler(repository.Object);
        var query = new GetProductByIdQuery(Guid.NewGuid());

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task Should_Return_Null_When_Product_Is_Inactive_And_IncludeInactive_Is_False()
    {
        var product = new Product(
            id: Guid.NewGuid(),
            sku: Sku.Create("TESTSKU"),
            name: "Inactive Product",
            price: Money.Create(10.0m, "USD"),
            categoryId: Guid.NewGuid(),
            supplierId: Guid.NewGuid(),
            description: "Description");
        product.Deactivate();

        var repository = new Mock<IProductRepository>();
        repository.Setup(x => x.GetByIdAsync(product.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var handler = new GetProductByIdQueryHandler(repository.Object);
        var query = new GetProductByIdQuery(product.Id, IncludeInactive: false);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task Should_Return_ProductResponse_When_Product_Is_Active()
    {
        var product = new Product(
            id: Guid.NewGuid(),
            sku: Sku.Create("TESTSKU"),
            name: "Active Product",
            price: Money.Create(10.0m, "USD"),
            categoryId: Guid.NewGuid(),
            supplierId: Guid.NewGuid(),
            description: "Description");

        var repository = new Mock<IProductRepository>();
        repository.Setup(x => x.GetByIdAsync(product.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var handler = new GetProductByIdQueryHandler(repository.Object);
        var query = new GetProductByIdQuery(product.Id);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().Be(product.Id);
        result.Sku.Should().Be("TESTSKU");
        result.Name.Should().Be("Active Product");
        result.Price.Should().Be(10.0m);
        result.Currency.Should().Be("USD");
        result.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task Should_Return_Inactive_Product_When_IncludeInactive_Is_True()
    {
        var product = new Product(
            id: Guid.NewGuid(),
            sku: Sku.Create("TESTSKU"),
            name: "Inactive Product",
            price: Money.Create(10.0m, "USD"),
            categoryId: Guid.NewGuid(),
            supplierId: Guid.NewGuid(),
            description: "Description");
        product.Deactivate();

        var repository = new Mock<IProductRepository>();
        repository.Setup(x => x.GetByIdAsync(product.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var handler = new GetProductByIdQueryHandler(repository.Object);
        var query = new GetProductByIdQuery(product.Id, IncludeInactive: true);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result!.IsActive.Should().BeFalse();
    }
}