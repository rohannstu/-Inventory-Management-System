using FluentValidation;
using InventoryManagementSystem.Domain.Enums;

namespace InventoryManagementSystem.Application.StockMovements.Commands.CreateStockMovement;

public class CreateStockMovementCommandValidator : AbstractValidator<CreateStockMovementCommand>
{
    public CreateStockMovementCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.");

        RuleFor(x => x.WarehouseId)
            .NotEmpty().WithMessage("WarehouseId is required.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be positive.");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("StockMovement type is required.");

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters.");
    }
}
