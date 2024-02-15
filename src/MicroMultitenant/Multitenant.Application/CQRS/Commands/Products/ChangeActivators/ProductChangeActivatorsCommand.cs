using MediatR;
using Multitenant.Application.CQRS.Commands.Products.ChangeActivators.Resources;

namespace Multitenant.Application.CQRS.Commands.Products.ChangeActivators

{
    public class ProductChangeActivatorsCommand : ProductChangeActivatorsRequest, IRequest<ProductChangeActivatorsResponse>
    {
        public string? Token { get; set; }
        public Guid Id { get; set; }


        public ProductChangeActivatorsCommand(Guid id)
        {
            Id = id;
        }

        public ProductChangeActivatorsCommand()
        {
        }
    }
}
