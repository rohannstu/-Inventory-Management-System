using InventoryManagementSystem.Application.Abstractions.Messaging;

namespace InventoryManagementSystem.Application.Products.Queries.GetProductById;

public record GetProductByIdQuery(Guid Id, bool IncludeInactive = false) : IQuery<ProductResponse?>;
