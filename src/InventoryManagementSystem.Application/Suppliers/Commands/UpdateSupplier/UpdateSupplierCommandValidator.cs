using FluentValidation;

namespace InventoryManagementSystem.Application.Suppliers.Commands.UpdateSupplier;

public class UpdateSupplierCommandValidator : AbstractValidator<UpdateSupplierCommand>
{
    public UpdateSupplierCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Supplier id is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Supplier name is required.")
            .MaximumLength(200).WithMessage("Supplier name cannot exceed 200 characters.");

        RuleFor(x => x.ContactEmail)
            .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.ContactEmail))
            .WithMessage("Contact email must be a valid email address.");

        RuleFor(x => x.ContactPhone)
            .MaximumLength(50).WithMessage("Contact phone cannot exceed 50 characters.");
    }
}
