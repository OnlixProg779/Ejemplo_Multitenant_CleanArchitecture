
using FluentValidation;

namespace Multitenant.Application.CQRS.Commands.Products.PatchProduct
{
    public class PatchProductCommandValidator : AbstractValidator<PatchProductCommand>
    {
        public PatchProductCommandValidator()
        {

        }
    }
}
