using InventoryManagementSystem.Application.Abstractions.Messaging;

namespace InventoryManagementSystem.Application.Categories.Queries.GetCategoryById;

public sealed record GetCategoryByIdQuery(Guid Id) : IQuery<CategoryResponse?>;
