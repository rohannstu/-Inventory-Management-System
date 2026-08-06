using FluentAssertions;
using InventoryManagementSystem.Domain.Entities;
using Xunit;

namespace InventoryManagementSystem.Tests.Domain;

public class SupplierTests
{
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        var id = Guid.NewGuid();
        var supplier = new Supplier(id, "Acme Corp", "contact@acme.com", "555-0100");

        supplier.Id.Should().Be(id);
        supplier.Name.Should().Be("Acme Corp");
        supplier.ContactEmail.Should().Be("contact@acme.com");
        supplier.ContactPhone.Should().Be("555-0100");
        supplier.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Constructor_Should_Set_ContactInfo_To_Null_When_Not_Provided()
    {
        var supplier = new Supplier(Guid.NewGuid(), "No Contact Corp");

        supplier.ContactEmail.Should().BeNull();
        supplier.ContactPhone.Should().BeNull();
    }

    [Fact]
    public void Constructor_Should_Throw_When_Name_Is_Empty()
    {
        Action act = () => new Supplier(Guid.NewGuid(), string.Empty);

        act.Should().Throw<ArgumentException>().WithMessage("Supplier name is required.*");
    }

    [Fact]
    public void Rename_Should_Update_Name()
    {
        var supplier = new Supplier(Guid.NewGuid(), "Acme Corp");

        supplier.Rename("Acme Incorporated");

        supplier.Name.Should().Be("Acme Incorporated");
    }

    [Fact]
    public void Rename_Should_Throw_When_Name_Is_Empty()
    {
        var supplier = new Supplier(Guid.NewGuid(), "Acme Corp");

        Action act = () => supplier.Rename(string.Empty);

        act.Should().Throw<ArgumentException>().WithMessage("Supplier name is required.*");
    }

    [Fact]
    public void UpdateContactInfo_Should_Update_Both_Fields()
    {
        var supplier = new Supplier(Guid.NewGuid(), "Acme Corp");

        supplier.UpdateContactInfo("new@acme.com", "555-0200");

        supplier.ContactEmail.Should().Be("new@acme.com");
        supplier.ContactPhone.Should().Be("555-0200");
    }

    [Fact]
    public void UpdateContactInfo_Should_Set_Fields_To_Null()
    {
        var supplier = new Supplier(Guid.NewGuid(), "Acme Corp", "contact@acme.com", "555-0100");

        supplier.UpdateContactInfo(null, null);

        supplier.ContactEmail.Should().BeNull();
        supplier.ContactPhone.Should().BeNull();
    }

    [Fact]
    public void Deactivate_Should_Set_IsActive_To_False()
    {
        var supplier = new Supplier(Guid.NewGuid(), "Acme Corp");

        supplier.Deactivate();

        supplier.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Activate_Should_Set_IsActive_To_True()
    {
        var supplier = new Supplier(Guid.NewGuid(), "Acme Corp");
        supplier.Deactivate();

        supplier.Activate();

        supplier.IsActive.Should().BeTrue();
    }
}