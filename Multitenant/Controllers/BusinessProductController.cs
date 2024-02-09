using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multitenant.Application.CQRS.Commands.Products.ChangeActivators;
using Multitenant.Application.CQRS.Commands.Products.ChangeActivators.Resources;
using Multitenant.Application.CQRS.Commands.Products.CreateProduct;
using Multitenant.Application.CQRS.Commands.Products.CreateProduct.Resources;
using Multitenant.Application.CQRS.Commands.Products.PatchProduct;
using Multitenant.Application.CQRS.Commands.Products.PatchProduct.Resources;
using Multitenant.Application.CQRS.Queries.Products.GetById;
using Multitenant.Application.CQRS.Queries.Products.GetPaginParams;
using Multitenant.Application.CQRS.Queries.Products.Vms;
using System.Net;

namespace Multitenant.Controllers
{
    [ApiController]
    [Route("api/0v1/[controller]")]
    public class BusinessProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BusinessProductController(IMediator mediator)
        {
            _mediator = mediator ??
                throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost(Name = "CreateProduct")]
        [HttpHead]
        [Consumes(
           "application/json")]
        [Authorize(Policy = "UserOrganization")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<CreateProductResponse>> CreateProduct([FromBody] CreateProductRequest request,
        [FromHeader(Name = "Authorization")] string? mediaTypeAuthorization)
        {
            var command = new CreateProductCommand()
            {
                Name = request.Name,
                Description = request.Description,
                Duration = request.Duration,
                Token = mediaTypeAuthorization
            };

            var VMresponse = await _mediator.Send(command);

            return VMresponse;
        }

        [Produces(
        "application/json")]
        [ProducesResponseType(typeof(ProductVm), (int)HttpStatusCode.OK)]
        [HttpGet("{id}", Name = "GetProduct")]
        [Authorize(Policy = "UserOrganization")]
        public async Task<ActionResult<ProductVm>> GetProduct(Guid id,
        [FromHeader(Name = "Authorization")] string? mediaTypeAuthorization)
        {
            var query = new GetProductByIdQuery(id);
            query.Token = mediaTypeAuthorization;
            var VMresponse = await _mediator.Send(query);

            return VMresponse;

        }

        [HttpPut("{id}/activator", Name = "ChangeActivatorProduct")]
        [HttpHead("{id}/activator")]
        [Consumes(
           "application/json")]
        [Authorize(Policy = "UserOrganization")]
        public async Task<ActionResult<ProductChangeActivatorsResponse>> ChangeActivatorProduct(Guid id, [FromBody] ProductChangeActivatorsRequest request,
           [FromHeader(Name = "Authorization")] string? mediaTypeAuthorization)
        {

            var command = new ProductChangeActivatorsCommand();

            command.Active = request.Active;
            command.Id = id;
            command.Token = mediaTypeAuthorization;

            var response = await _mediator.Send(command);

            if (response.ResponseChange == 0)
            {
                return NotFound(response.ResponseMessage);
            }
            return Ok(response);

        }

        [HttpPut("{id}/patchProduct", Name = "PatchProduct")]
        [HttpHead("{id}/patchProduct")]
        [Consumes(
           "application/json")]
        [Authorize(Policy = "UserOrganization")]
        public async Task<ActionResult<PatchProductResponse>> PatchProduct(Guid id, [FromBody] PatchProductRequest toPatch,
         [FromHeader(Name = "Authorization")] string? mediaTypeAuthorization)
        {

            var command = new PatchProductCommand
            {
                Id = id,
                Token = mediaTypeAuthorization
            };

            if (!string.IsNullOrWhiteSpace(toPatch.Name))
            {
                command.patchEntity.Replace(e => e.Name, toPatch.Name);
            }

            if (!string.IsNullOrWhiteSpace(toPatch.Description))
            {
                command.patchEntity.Replace(e => e.Description, toPatch.Description);
            }

            if (!string.IsNullOrWhiteSpace(toPatch.Duration))
            {
                command.patchEntity.Replace(e => e.Duration, toPatch.Duration);
            }

            var VMresponse = await _mediator.Send(command);

            return VMresponse;

        }

        [HttpGet("pagination", Name = "GetProducts")]
        [HttpHead("pagination")]
        [Produces(
           "application/json")]
        [Authorize(Policy = "UserOrganization")]
        [ProducesResponseType(typeof(PaginationVm<ProductVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginationVm<ProductVm>>> GetPaginationAgencies(
           [FromQuery] GetProductPaginParamsQuery entityWParams,
           [FromHeader(Name = "Accept")] string mediaType,
           [FromHeader(Name = "Authorization")] string? mediaTypeAuthorization
          )
        {

            if (!string.IsNullOrWhiteSpace(mediaTypeAuthorization)) entityWParams.Token = mediaTypeAuthorization;

            var paginationResponse = await _mediator.Send(entityWParams);

            if (paginationResponse.ResponseAction == 0)
            {
                return NotFound(paginationResponse.ResponseMessages);
            }

            return Ok(paginationResponse);
        }


    }
}
