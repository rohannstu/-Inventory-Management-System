using FluentAssertions;
using InventoryManagementSystem.Application.Abstractions.Persistence;
using InventoryManagementSystem.Application.Abstractions.Pagination;
using InventoryManagementSystem.Application.StockMovements.Queries.GetStockMovementsList;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Enums;
using Moq;
using Xunit;

namespace InventoryManagementSystem.Tests.StockMovements.Queries;

public class GetStockMovementsListQueryHandlerTests
{
    [Fact]
    public async Task Should_Return_PagedResult_With_StockMovements()
    {
        var movements = new List<StockMovement>
        {
            StockMovement.CreateStockIn(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 10, null),
            StockMovement.CreateStockOut(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 5, null)
        };

        var repository = new Mock<IStockMovementRepository>();
        repository.Setup(x => x.GetPagedAsync(It.IsAny<StockMovementListFilter>(), It.IsAny<PaginationParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((movements, 2));

        var handler = new GetStockMovementsListQueryHandler(repository.Object);
        var query = new GetStockMovementsListQuery(
            new StockMovementListFilter(),
            new PaginationParams { Page = 1, PageSize = 20 });

        var result = await handler.Handle(query, CancellationToken.None);

        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
    }

    [Fact]
    public async Task Should_Return_Empty_Result_When_No_Movements()
    {
        var repository = new Mock<IStockMovementRepository>();
        repository.Setup(x => x.GetPagedAsync(It.IsAny<StockMovementListFilter>(), It.IsAny<PaginationParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((new List<StockMovement>(), 0));

        var handler = new GetStockMovementsListQueryHandler(repository.Object);
        var query = new GetStockMovementsListQuery(
            new StockMovementListFilter(),
            new PaginationParams { Page = 1, PageSize = 20 });

        var result = await handler.Handle(query, CancellationToken.None);

        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task Should_Map_StockMovement_Properties_Correctly()
    {
        var productId = Guid.NewGuid();
        var warehouseId = Guid.NewGuid();
        var movement = StockMovement.CreateStockIn(Guid.NewGuid(), productId, warehouseId, 10, null, "Test notes");

        var repository = new Mock<IStockMovementRepository>();
        repository.Setup(x => x.GetPagedAsync(It.IsAny<StockMovementListFilter>(), It.IsAny<PaginationParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((new List<StockMovement> { movement }, 1));

        var handler = new GetStockMovementsListQueryHandler(repository.Object);
        var query = new GetStockMovementsListQuery(
            new StockMovementListFilter(),
            new PaginationParams { Page = 1, PageSize = 20 });

        var result = await handler.Handle(query, CancellationToken.None);

        result.Items.Should().HaveCount(1);
        var item = result.Items.First();
        item.ProductId.Should().Be(productId);
        item.WarehouseId.Should().Be(warehouseId);
        item.Type.Should().Be(StockMovementType.StockIn);
        item.Quantity.Should().Be(10);
        item.Notes.Should().Be("Test notes");
    }
}