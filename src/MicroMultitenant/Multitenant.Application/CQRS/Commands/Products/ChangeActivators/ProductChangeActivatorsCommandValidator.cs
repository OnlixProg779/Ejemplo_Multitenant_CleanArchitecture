using FluentValidation;

namespace Multitenant.Application.CQRS.Commands.Products.ChangeActivators

{
    public class ProductChangeActivatorsCommandValidator : AbstractValidator<ProductChangeActivatorsCommand>
    {
        public ProductChangeActivatorsCommandValidator()
        {
            RuleFor(a => a.Active).NotNull();
            RuleFor(a => a.Id).NotNull();
        }
    }
}
