using FluentAssertions;
using InventoryManagementSystem.Application.Abstractions.Persistence;
using InventoryManagementSystem.Application.Categories.Queries.GetCategoryById;
using InventoryManagementSystem.Domain.Entities;
using Moq;
using Xunit;

namespace InventoryManagementSystem.Tests.Categories.Queries;

public class GetCategoryByIdQueryHandlerTests
{
    [Fact]
    public async Task Should_Return_Null_When_Category_Not_Found()
    {
        var repository = new Mock<ICategoryRepository>();
        repository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category?)null);

        var handler = new GetCategoryByIdQueryHandler(repository.Object);
        var query = new GetCategoryByIdQuery(Guid.NewGuid());

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task Should_Return_CategoryResponse_When_Category_Exists()
    {
        var category = new Category(Guid.NewGuid(), "Electronics", "Electronic devices");

        var repository = new Mock<ICategoryRepository>();
        repository.Setup(x => x.GetByIdAsync(category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        var handler = new GetCategoryByIdQueryHandler(repository.Object);
        var query = new GetCategoryByIdQuery(category.Id);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().Be(category.Id);
        result.Name.Should().Be("Electronics");
        result.Description.Should().Be("Electronic devices");
    }

    [Fact]
    public async Task Should_Return_Category_With_Null_Description_When_Not_Provided()
    {
        var category = new Category(Guid.NewGuid(), "Uncategorized");

        var repository = new Mock<ICategoryRepository>();
        repository.Setup(x => x.GetByIdAsync(category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        var handler = new GetCategoryByIdQueryHandler(repository.Object);
        var query = new GetCategoryByIdQuery(category.Id);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Description.Should().BeNull();
    }
}