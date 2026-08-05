using FluentValidation;

namespace InventoryManagementSystem.Application.Products;

public static class ProductValidationRules
{
    public static IRuleBuilderOptions<T, string> ProductName<T>(this IRuleBuilder<T, string> rule) =>
        rule.NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(200).WithMessage("Product name cannot exceed 200 characters.");

    public static IRuleBuilderOptions<T, string?> ProductDescription<T>(this IRuleBuilder<T, string?> rule) =>
        rule.MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

    public static IRuleBuilderOptions<T, decimal> ProductPrice<T>(this IRuleBuilder<T, decimal> rule) =>
        rule.GreaterThanOrEqualTo(0).WithMessage("Price cannot be negative.");

    public static IRuleBuilderOptions<T, string> CurrencyCode<T>(this IRuleBuilder<T, string> rule) =>
        rule.NotEmpty().WithMessage("Currency is required.")
            .Length(3).WithMessage("Currency must be a 3-letter ISO code.");
}
