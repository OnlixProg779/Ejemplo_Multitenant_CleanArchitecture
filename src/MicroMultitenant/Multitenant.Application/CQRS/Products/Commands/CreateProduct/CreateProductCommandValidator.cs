using FluentValidation;

namespace Multitenant.Application.CQRS.Products.Commands.CreateProduct

{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(data => data.Name).NotEmpty().NotNull();
            RuleFor(data => data.Duration).NotEmpty().NotNull();
            RuleFor(data => data.Description).NotEmpty().NotNull();
        }
    }
}
