
using FluentValidation;

namespace Multitenant.Application.CQRS.Products.Commands.PatchProduct
{
    public class PatchProductCommandValidator : AbstractValidator<PatchProductCommand>
    {
        public PatchProductCommandValidator()
        {

        }
    }
}
