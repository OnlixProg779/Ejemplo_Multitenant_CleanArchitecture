using Base.Application.CQRS.Queries.Vms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multitenant.Application.CQRS.Products.Commands.ChangeActivators;
using Multitenant.Application.CQRS.Products.Commands.ChangeActivators.Resources;
using Multitenant.Application.CQRS.Products.Commands.CreateProduct;
using Multitenant.Application.CQRS.Products.Commands.CreateProduct.Resources;
using Multitenant.Application.CQRS.Products.Commands.PatchProduct;
using Multitenant.Application.CQRS.Products.Commands.PatchProduct.Resources;
using Multitenant.Application.CQRS.Products.Queries.GetById;
using Multitenant.Application.CQRS.Products.Queries.GetPaginParams;
using Multitenant.Application.CQRS.Products.Queries.Vms;
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

        [HttpPost("CreateProduct/company/{organization}", Name = "CreateProduct")]
        [HttpHead("CreateProduct/company/{organization}")]
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
        [HttpGet("GetProduct/{id}/company/{organization}", Name = "GetProduct")]
        [HttpHead("GetProduct/{id}/company/{organization}")]
        [Authorize(Policy = "UserOrganization")]
        public async Task<ActionResult<ProductVm>> GetProduct(Guid id,
        [FromHeader(Name = "Authorization")] string? mediaTypeAuthorization)
        {
            var query = new GetProductByIdQuery(id);
            query.Token = mediaTypeAuthorization;
            var VMresponse = await _mediator.Send(query);

            return VMresponse;

        }

        [HttpPut("{id}/activator/company/{organization}", Name = "ChangeActivatorProduct")]
        [HttpHead("{id}/activator/company/{organization}")]
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

        [HttpPut("{id}/patchProduct/company/{organization}", Name = "PatchProduct")]
        [HttpHead("{id}/patchProduct/company/{organization}")]
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

        [HttpGet("pagination/company/{organization}", Name = "GetProducts")]
        [HttpHead("pagination/company/{organization}")]
        [Produces(
           "application/json")]
        [Authorize(Policy = "UserOrganization")]
        [ProducesResponseType(typeof(PaginationVm<ProductVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginationVm<ProductVm>>> GetPaginationAgencies(
           [FromQuery] GetProductPaginParamsQuery entityWParams,
           [FromHeader(Name = "Authorization")] string? mediaTypeAuthorization
          )
        {

            if (!string.IsNullOrWhiteSpace(mediaTypeAuthorization)) entityWParams.Token = mediaTypeAuthorization;

            var paginationResponse = await _mediator.Send(entityWParams);

            if (paginationResponse.ResponseAction == 0)
            {
                return NotFound(paginationResponse.ResponseMessages);
            }

            var paginationMetadata = new
            {
                totalCount = paginationResponse.TotalCount,
                pageSize = paginationResponse.PageSize,
                currentPage = paginationResponse.CurrentPage,
                totalPages = paginationResponse.TotalPages
            };

            return Ok(new { items = paginationResponse, pagination = paginationMetadata });
        }

        [HttpOptions]
        public IActionResult GetControllerOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST,PATCH,HEAD");
            return Ok();
        }
    }
}
