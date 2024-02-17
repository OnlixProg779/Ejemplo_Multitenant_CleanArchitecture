using Base.Application.CQRS.Commands;

namespace Multitenant.Application.CQRS.Products.Commands.ChangeActivators.Resources
{
    public class ProductChangeActivatorsResponse : ChangeActivatorsResponse
    {
        public ProductChangeActivatorsResponse(ChangeActivatorsResponse entity)
        {
            this.NewValue = entity.NewValue;
            this.ResponseMessage = entity.ResponseMessage;
            this.ResponseChange = entity.ResponseChange;
        }
    }
}
