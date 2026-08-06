using FluentAssertions;
using InventoryManagementSystem.Application.Abstractions.Persistence;
using InventoryManagementSystem.Application.Abstractions.Pagination;
using InventoryManagementSystem.Application.Products.Queries.GetProductsList;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.ValueObjects;
using Moq;
using Xunit;

namespace InventoryManagementSystem.Tests.Products.Queries;

public class GetProductsListQueryHandlerTests
{
    [Fact]
    public async Task Should_Return_PagedResult_With_Products()
    {
        var products = new List<Product>
        {
            new Product(
                id: Guid.NewGuid(),
                sku: Sku.Create("SKU1"),
                name: "Product 1",
                price: Money.Create(10.0m, "USD"),
                categoryId: Guid.NewGuid(),
                supplierId: Guid.NewGuid()),
            new Product(
                id: Guid.NewGuid(),
                sku: Sku.Create("SKU2"),
                name: "Product 2",
                price: Money.Create(20.0m, "USD"),
                categoryId: Guid.NewGuid(),
                supplierId: Guid.NewGuid())
        };

        var repository = new Mock<IProductRepository>();
        repository.Setup(x => x.GetPagedAsync(It.IsAny<ProductListFilter>(), It.IsAny<PaginationParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((products, 2));

        var handler = new GetProductsListQueryHandler(repository.Object);
        var query = new GetProductsListQuery(
            new ProductListFilter(),
            new PaginationParams { Page = 1, PageSize = 20 });

        var result = await handler.Handle(query, CancellationToken.None);

        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(20);
    }

    [Fact]
    public async Task Should_Return_Empty_Result_When_No_Products()
    {
        var repository = new Mock<IProductRepository>();
        repository.Setup(x => x.GetPagedAsync(It.IsAny<ProductListFilter>(), It.IsAny<PaginationParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((new List<Product>(), 0));

        var handler = new GetProductsListQueryHandler(repository.Object);
        var query = new GetProductsListQuery(
            new ProductListFilter(),
            new PaginationParams { Page = 1, PageSize = 20 });

        var result = await handler.Handle(query, CancellationToken.None);

        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task Should_Pass_Filter_And_Pagination_To_Repository()
    {
        var products = new List<Product>();

        var repository = new Mock<IProductRepository>();
        repository.Setup(x => x.GetPagedAsync(It.IsAny<ProductListFilter>(), It.IsAny<PaginationParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((products, 0));

        var handler = new GetProductsListQueryHandler(repository.Object);
        var filter = new ProductListFilter { CategoryId = Guid.NewGuid(), IsActive = true, SearchTerm = "test" };
        var pagination = new PaginationParams { Page = 2, PageSize = 10, SortBy = "name", SortDescending = true };
        var query = new GetProductsListQuery(filter, pagination);

        await handler.Handle(query, CancellationToken.None);

        repository.Verify(x => x.GetPagedAsync(
            It.Is<ProductListFilter>(f => f.CategoryId == filter.CategoryId && f.IsActive == true && f.SearchTerm == "test"),
            It.Is<PaginationParams>(p => p.Page == 2 && p.PageSize == 10 && p.SortBy == "name" && p.SortDescending == true),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}