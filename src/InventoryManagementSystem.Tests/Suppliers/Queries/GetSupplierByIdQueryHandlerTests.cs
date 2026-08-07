using FluentAssertions;
using InventoryManagementSystem.Application.Abstractions.Persistence;
using InventoryManagementSystem.Application.Suppliers.Queries.GetSupplierById;
using InventoryManagementSystem.Domain.Entities;
using Moq;
using Xunit;

namespace InventoryManagementSystem.Tests.Suppliers.Queries;

public class GetSupplierByIdQueryHandlerTests
{
    [Fact]
    public async Task Should_Return_Null_When_Supplier_Not_Found()
    {
        var repository = new Mock<ISupplierRepository>();
        repository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Supplier?)null);

        var handler = new GetSupplierByIdQueryHandler(repository.Object);
        var query = new GetSupplierByIdQuery(Guid.NewGuid());

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task Should_Return_SupplierResponse_When_Supplier_Exists()
    {
        var supplier = new Supplier(Guid.NewGuid(), "Acme Corp", "contact@acme.com", "555-0100");

        var repository = new Mock<ISupplierRepository>();
        repository.Setup(x => x.GetByIdAsync(supplier.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(supplier);

        var handler = new GetSupplierByIdQueryHandler(repository.Object);
        var query = new GetSupplierByIdQuery(supplier.Id);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().Be(supplier.Id);
        result.Name.Should().Be("Acme Corp");
        result.ContactEmail.Should().Be("contact@acme.com");
        result.ContactPhone.Should().Be("555-0100");
        result.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task Should_Return_Null_ContactInfo_When_Not_Provided()
    {
        var supplier = new Supplier(Guid.NewGuid(), "No Contact Corp");

        var repository = new Mock<ISupplierRepository>();
        repository.Setup(x => x.GetByIdAsync(supplier.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(supplier);

        var handler = new GetSupplierByIdQueryHandler(repository.Object);
        var query = new GetSupplierByIdQuery(supplier.Id);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result!.ContactEmail.Should().BeNull();
        result.ContactPhone.Should().BeNull();
    }
}