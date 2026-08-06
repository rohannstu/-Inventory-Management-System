using FluentAssertions;
using InventoryManagementSystem.Application.Abstractions.Persistence;
using InventoryManagementSystem.Application.StockMovements.Queries.GetStockMovementById;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Enums;
using Moq;
using Xunit;

namespace InventoryManagementSystem.Tests.StockMovements.Queries;

public class GetStockMovementByIdQueryHandlerTests
{
    [Fact]
    public async Task Should_Return_Null_When_Movement_Not_Found()
    {
        var repository = new Mock<IStockMovementRepository>();
        repository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((StockMovement?)null);

        var handler = new GetStockMovementByIdQueryHandler(repository.Object);
        var query = new GetStockMovementByIdQuery(Guid.NewGuid());

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task Should_Return_StockMovementResponse_When_Movement_Exists()
    {
        var productId = Guid.NewGuid();
        var warehouseId = Guid.NewGuid();
        var movement = StockMovement.CreateStockOut(Guid.NewGuid(), productId, warehouseId, 5, null, "Adjustment");

        var repository = new Mock<IStockMovementRepository>();
        repository.Setup(x => x.GetByIdAsync(movement.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(movement);

        var handler = new GetStockMovementByIdQueryHandler(repository.Object);
        var query = new GetStockMovementByIdQuery(movement.Id);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().Be(movement.Id);
        result.ProductId.Should().Be(productId);
        result.WarehouseId.Should().Be(warehouseId);
        result.Type.Should().Be(StockMovementType.StockOut);
        result.Quantity.Should().Be(5);
        result.Notes.Should().Be("Adjustment");
    }
}