using FluentValidation;
using InventoryManagementSystem.Application.Products;

namespace InventoryManagementSystem.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Name).ProductName();
        RuleFor(x => x.Description).ProductDescription();
        RuleFor(x => x.Price).ProductPrice();
        RuleFor(x => x.Currency).CurrencyCode();

        RuleFor(x => x.CategoryId)
            .NotEmpty();

        RuleFor(x => x.SupplierId)
            .NotEmpty();
    }
}
