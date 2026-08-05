using FluentValidation;

namespace InventoryManagementSystem.Application.Auth.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Role).NotEmpty().Must(r => Enum.TryParse<Domain.Enums.UserRole>(r, out _))
            .WithMessage("Role must be one of: Staff, Manager, Admin.");
    }
}
