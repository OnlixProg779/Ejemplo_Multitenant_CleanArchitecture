using FluentValidation;

namespace DemoAuth.Application.CQRS.Identity.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("El nombre de usuario no puede estar vacío.")
            .NotNull().WithMessage("El nombre de usuario es requerido.")
            .MinimumLength(3).WithMessage("El nombre de usuario debe tener al menos 3 caracteres.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña no puede estar vacía.")
                .NotNull().WithMessage("La contraseña es requerida.")
                .MinimumLength(3).WithMessage("La contraseña debe tener al menos 3 caracteres.")
                .Matches(@"\d").WithMessage("La contraseña debe contener al menos un dígito.")
                .When(x => !string.IsNullOrEmpty(x.Password));

            RuleFor(x => x.Rol)
                .NotEmpty().WithMessage("El rol no puede estar vacío.")
                .NotNull().WithMessage("El rol es requerido.");

            RuleFor(x => x.OrganizationName)
           .NotEmpty().WithMessage("La referencia no puede estar vacío.")
           .NotNull().WithMessage("La referencia es requerido.");

            RuleFor(x => x.Token)
          .NotEmpty().WithMessage("El token no puede estar vacío.")
          .NotNull().WithMessage("El token es requerido.");

        }
    }
}
