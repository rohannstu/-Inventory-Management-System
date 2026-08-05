using FluentValidation;

namespace InventoryManagementSystem.Application.Warehouses.Commands.UpdateWarehouse;

public class UpdateWarehouseCommandValidator : AbstractValidator<UpdateWarehouseCommand>
{
    public UpdateWarehouseCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Warehouse id is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Warehouse name is required.")
            .MaximumLength(200).WithMessage("Warehouse name cannot exceed 200 characters.");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Warehouse location is required.")
            .MaximumLength(500).WithMessage("Warehouse location cannot exceed 500 characters.");
    }
}
