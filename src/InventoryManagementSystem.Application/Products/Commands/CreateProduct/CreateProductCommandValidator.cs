using FluentValidation;
using InventoryManagementSystem.Application.Products;

namespace InventoryManagementSystem.Application.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Sku)
            .NotEmpty().WithMessage("SKU is required.")
            .MaximumLength(32).WithMessage("SKU cannot exceed 32 characters.");

        RuleFor(x => x.Name).ProductName();
        RuleFor(x => x.Description).ProductDescription();
        RuleFor(x => x.Price).ProductPrice();
        RuleFor(x => x.Currency).CurrencyCode();

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("CategoryId is required.");

        RuleFor(x => x.SupplierId)
            .NotEmpty().WithMessage("SupplierId is required.");
    }
}
