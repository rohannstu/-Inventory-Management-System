using FluentValidation;

namespace InventoryManagementSystem.Application.Warehouses.Commands.CreateWarehouse;

public class CreateWarehouseCommandValidator : AbstractValidator<CreateWarehouseCommand>
{
    public CreateWarehouseCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Warehouse name is required.")
            .MaximumLength(200).WithMessage("Warehouse name cannot exceed 200 characters.");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Warehouse location is required.")
            .MaximumLength(500).WithMessage("Warehouse location cannot exceed 500 characters.");
    }
}
