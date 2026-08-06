using InventoryManagementSystem.Domain.Enums;

namespace InventoryManagementSystem.Application.StockMovements.Queries.GetStockMovementsList;

public class StockMovementListFilter
{
    public Guid? ProductId { get; set; }
    public Guid? WarehouseId { get; set; }
    public StockMovementType? Type { get; set; }
}
