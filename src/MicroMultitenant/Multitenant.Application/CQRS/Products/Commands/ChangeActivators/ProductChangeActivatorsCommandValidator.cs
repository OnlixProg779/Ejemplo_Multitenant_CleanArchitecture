using FluentValidation;

namespace Multitenant.Application.CQRS.Products.Commands.ChangeActivators

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
