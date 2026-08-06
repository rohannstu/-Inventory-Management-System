using FluentAssertions;
using InventoryManagementSystem.Domain.Entities;
using Xunit;

namespace InventoryManagementSystem.Tests.Domain;

public class WarehouseTests
{
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        var id = Guid.NewGuid();
        var warehouse = new Warehouse(id, "Main Warehouse", "New York");

        warehouse.Id.Should().Be(id);
        warehouse.Name.Should().Be("Main Warehouse");
        warehouse.Location.Should().Be("New York");
        warehouse.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Constructor_Should_Throw_When_Name_Is_Empty()
    {
        Action act = () => new Warehouse(Guid.NewGuid(), string.Empty, "New York");

        act.Should().Throw<ArgumentException>().WithMessage("Warehouse name is required.*");
    }

    [Fact]
    public void Constructor_Should_Throw_When_Location_Is_Empty()
    {
        Action act = () => new Warehouse(Guid.NewGuid(), "Main Warehouse", string.Empty);

        act.Should().Throw<ArgumentException>().WithMessage("Warehouse location is required.*");
    }

    [Fact]
    public void Rename_Should_Update_Name()
    {
        var warehouse = new Warehouse(Guid.NewGuid(), "Main Warehouse", "New York");

        warehouse.Rename("Secondary Warehouse");

        warehouse.Name.Should().Be("Secondary Warehouse");
    }

    [Fact]
    public void Rename_Should_Throw_When_Name_Is_Empty()
    {
        var warehouse = new Warehouse(Guid.NewGuid(), "Main Warehouse", "New York");

        Action act = () => warehouse.Rename(string.Empty);

        act.Should().Throw<ArgumentException>().WithMessage("Warehouse name is required.*");
    }

    [Fact]
    public void Relocate_Should_Update_Location()
    {
        var warehouse = new Warehouse(Guid.NewGuid(), "Main Warehouse", "New York");

        warehouse.Relocate("Chicago");

        warehouse.Location.Should().Be("Chicago");
    }

    [Fact]
    public void Relocate_Should_Throw_When_Location_Is_Empty()
    {
        var warehouse = new Warehouse(Guid.NewGuid(), "Main Warehouse", "New York");

        Action act = () => warehouse.Relocate(string.Empty);

        act.Should().Throw<ArgumentException>().WithMessage("Warehouse location is required.*");
    }

    [Fact]
    public void Deactivate_Should_Set_IsActive_To_False()
    {
        var warehouse = new Warehouse(Guid.NewGuid(), "Main Warehouse", "New York");

        warehouse.Deactivate();

        warehouse.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Activate_Should_Set_IsActive_To_True()
    {
        var warehouse = new Warehouse(Guid.NewGuid(), "Main Warehouse", "New York");
        warehouse.Deactivate();

        warehouse.Activate();

        warehouse.IsActive.Should().BeTrue();
    }
}