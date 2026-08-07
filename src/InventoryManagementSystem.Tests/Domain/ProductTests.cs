using FluentAssertions;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.ValueObjects;
using Xunit;

namespace InventoryManagementSystem.Tests.Domain;

public class ProductTests
{
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        var id = Guid.NewGuid();
        var sku = Sku.Create("TESTSKU");
        var price = Money.Create(10.0m, "USD");
        var categoryId = Guid.NewGuid();
        var supplierId = Guid.NewGuid();

        var product = new Product(id, sku, "Test Product", price, categoryId, supplierId, "Description");

        product.Id.Should().Be(id);
        product.Sku.Should().Be(sku);
        product.Name.Should().Be("Test Product");
        product.Description.Should().Be("Description");
        product.Price.Should().Be(price);
        product.StockQuantity.Should().Be(0);
        product.IsActive.Should().BeTrue();
        product.CategoryId.Should().Be(categoryId);
        product.SupplierId.Should().Be(supplierId);
    }

    [Fact]
    public void Constructor_Should_Throw_When_Name_Is_Empty()
    {
        var sku = Sku.Create("TESTSKU");
        var price = Money.Create(10.0m, "USD");

        Action act = () => new Product(Guid.NewGuid(), sku, string.Empty, price, Guid.NewGuid(), Guid.NewGuid());

        act.Should().Throw<ArgumentException>().WithMessage("Product name is required.*");
    }

    [Fact]
    public void Constructor_Should_Throw_When_Sku_Is_Null()
    {
        var price = Money.Create(10.0m, "USD");

        Action act = () => new Product(Guid.NewGuid(), null!, "Test", price, Guid.NewGuid(), Guid.NewGuid());

        act.Should().Throw<ArgumentNullException>().WithParameterName("sku");
    }

    [Fact]
    public void Constructor_Should_Throw_When_Price_Is_Null()
    {
        var sku = Sku.Create("TESTSKU");

        Action act = () => new Product(Guid.NewGuid(), sku, "Test", null!, Guid.NewGuid(), Guid.NewGuid());

        act.Should().Throw<ArgumentNullException>().WithParameterName("price");
    }

    [Fact]
    public void Rename_Should_Update_Name()
    {
        var product = CreateProduct();

        product.Rename("New Name");

        product.Name.Should().Be("New Name");
    }

    [Fact]
    public void Rename_Should_Throw_When_Name_Is_Empty()
    {
        var product = CreateProduct();

        Action act = () => product.Rename(string.Empty);

        act.Should().Throw<ArgumentException>().WithMessage("Product name is required.*");
    }

    [Fact]
    public void UpdateDescription_Should_Update_Description()
    {
        var product = CreateProduct();

        product.UpdateDescription("New Description");

        product.Description.Should().Be("New Description");
    }

    [Fact]
    public void UpdateDescription_Should_Set_To_Null()
    {
        var product = CreateProduct();
        product.UpdateDescription("Has Description");

        product.UpdateDescription(null);

        product.Description.Should().BeNull();
    }

    [Fact]
    public void ChangePrice_Should_Update_Price()
    {
        var product = CreateProduct();
        var newPrice = Money.Create(25.0m, "USD");

        product.ChangePrice(newPrice);

        product.Price.Should().Be(newPrice);
    }

    [Fact]
    public void ChangePrice_Should_Throw_When_Price_Is_Null()
    {
        var product = CreateProduct();

        Action act = () => product.ChangePrice(null!);

        act.Should().Throw<ArgumentNullException>().WithParameterName("newPrice");
    }

    [Fact]
    public void ChangeCategory_Should_Update_CategoryId()
    {
        var product = CreateProduct();
        var newCategoryId = Guid.NewGuid();

        product.ChangeCategory(newCategoryId);

        product.CategoryId.Should().Be(newCategoryId);
    }

    [Fact]
    public void ChangeSupplier_Should_Update_SupplierId()
    {
        var product = CreateProduct();
        var newSupplierId = Guid.NewGuid();

        product.ChangeSupplier(newSupplierId);

        product.SupplierId.Should().Be(newSupplierId);
    }

    [Fact]
    public void UpdateDetails_Should_Update_All_Fields()
    {
        var product = CreateProduct();
        var newPrice = Money.Create(30.0m, "USD");
        var newCategoryId = Guid.NewGuid();
        var newSupplierId = Guid.NewGuid();

        product.UpdateDetails("Updated Name", "Updated Description", newPrice, newCategoryId, newSupplierId);

        product.Name.Should().Be("Updated Name");
        product.Description.Should().Be("Updated Description");
        product.Price.Should().Be(newPrice);
        product.CategoryId.Should().Be(newCategoryId);
        product.SupplierId.Should().Be(newSupplierId);
    }

    [Fact]
    public void IncreaseStock_Should_Add_To_StockQuantity()
    {
        var product = CreateProduct();

        product.IncreaseStock(10);

        product.StockQuantity.Should().Be(10);
    }

    [Fact]
    public void IncreaseStock_Should_Throw_When_Quantity_Is_Not_Positive()
    {
        var product = CreateProduct();

        Action act = () => product.IncreaseStock(0);

        act.Should().Throw<ArgumentException>().WithMessage("Quantity to increase must be positive.*");
    }

    [Fact]
    public void DecreaseStock_Should_Subtract_From_StockQuantity()
    {
        var product = CreateProduct();
        product.IncreaseStock(10);

        product.DecreaseStock(3);

        product.StockQuantity.Should().Be(7);
    }

    [Fact]
    public void DecreaseStock_Should_Throw_When_Quantity_Is_Not_Positive()
    {
        var product = CreateProduct();

        Action act = () => product.DecreaseStock(0);

        act.Should().Throw<ArgumentException>().WithMessage("Quantity to decrease must be positive.*");
    }

    [Fact]
    public void DecreaseStock_Should_Throw_When_Insufficient_Stock()
    {
        var product = CreateProduct();
        product.IncreaseStock(5);

        Action act = () => product.DecreaseStock(10);

        act.Should().Throw<InvalidOperationException>().WithMessage("Cannot decrease stock by 10; only 5 available.");
    }

    [Fact]
    public void Deactivate_Should_Set_IsActive_To_False()
    {
        var product = CreateProduct();

        product.Deactivate();

        product.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Activate_Should_Set_IsActive_To_True()
    {
        var product = CreateProduct();
        product.Deactivate();

        product.Activate();

        product.IsActive.Should().BeTrue();
    }

    private static Product CreateProduct()
    {
        return new Product(
            Guid.NewGuid(),
            Sku.Create("TESTSKU"),
            "Test Product",
            Money.Create(10.0m, "USD"),
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Description");
    }
}