using FluentAssertions;
using InventoryManagementSystem.Application.Abstractions.Persistence;
using InventoryManagementSystem.Application.Warehouses.Queries.GetWarehouseById;
using InventoryManagementSystem.Domain.Entities;
using Moq;
using Xunit;

namespace InventoryManagementSystem.Tests.Warehouses.Queries;

public class GetWarehouseByIdQueryHandlerTests
{
    [Fact]
    public async Task Should_Return_Null_When_Warehouse_Not_Found()
    {
        var repository = new Mock<IWarehouseRepository>();
        repository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Warehouse?)null);

        var handler = new GetWarehouseByIdQueryHandler(repository.Object);
        var query = new GetWarehouseByIdQuery(Guid.NewGuid());

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task Should_Return_WarehouseResponse_When_Warehouse_Exists()
    {
        var warehouse = new Warehouse(Guid.NewGuid(), "Main Warehouse", "New York");

        var repository = new Mock<IWarehouseRepository>();
        repository.Setup(x => x.GetByIdAsync(warehouse.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(warehouse);

        var handler = new GetWarehouseByIdQueryHandler(repository.Object);
        var query = new GetWarehouseByIdQuery(warehouse.Id);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().Be(warehouse.Id);
        result.Name.Should().Be("Main Warehouse");
        result.Location.Should().Be("New York");
        result.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task Should_Return_Inactive_Warehouse_Response()
    {
        var warehouse = new Warehouse(Guid.NewGuid(), "Closed Warehouse", "Chicago");
        warehouse.Deactivate();

        var repository = new Mock<IWarehouseRepository>();
        repository.Setup(x => x.GetByIdAsync(warehouse.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(warehouse);

        var handler = new GetWarehouseByIdQueryHandler(repository.Object);
        var query = new GetWarehouseByIdQuery(warehouse.Id);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result!.IsActive.Should().BeFalse();
    }
}