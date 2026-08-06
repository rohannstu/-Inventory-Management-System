using FluentAssertions;
using InventoryManagementSystem.Domain.Entities;
using Xunit;

namespace InventoryManagementSystem.Tests.Domain;

public class CategoryTests
{
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        var id = Guid.NewGuid();
        var category = new Category(id, "Electronics", "Electronic devices");

        category.Id.Should().Be(id);
        category.Name.Should().Be("Electronics");
        category.Description.Should().Be("Electronic devices");
    }

    [Fact]
    public void Constructor_Should_Set_Description_To_Null_When_Not_Provided()
    {
        var category = new Category(Guid.NewGuid(), "Uncategorized");

        category.Description.Should().BeNull();
    }

    [Fact]
    public void Constructor_Should_Throw_When_Name_Is_Empty()
    {
        Action act = () => new Category(Guid.NewGuid(), string.Empty);

        act.Should().Throw<ArgumentException>().WithMessage("Category name is required.*");
    }

    [Fact]
    public void Rename_Should_Update_Name()
    {
        var category = new Category(Guid.NewGuid(), "Electronics");

        category.Rename("Electronics & Gadgets");

        category.Name.Should().Be("Electronics & Gadgets");
    }

    [Fact]
    public void Rename_Should_Throw_When_Name_Is_Empty()
    {
        var category = new Category(Guid.NewGuid(), "Electronics");

        Action act = () => category.Rename(string.Empty);

        act.Should().Throw<ArgumentException>().WithMessage("Category name is required.*");
    }

    [Fact]
    public void UpdateDescription_Should_Update_Description()
    {
        var category = new Category(Guid.NewGuid(), "Electronics");

        category.UpdateDescription("Updated description");

        category.Description.Should().Be("Updated description");
    }

    [Fact]
    public void UpdateDescription_Should_Set_To_Null()
    {
        var category = new Category(Guid.NewGuid(), "Electronics", "Has description");

        category.UpdateDescription(null);

        category.Description.Should().BeNull();
    }
}