using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multitenant.Application.CQRS.Business.Commands.InitOrganization;
using Multitenant.Application.CQRS.Business.Commands.InitOrganization.Resources;
using System.Net;

namespace Multitenant.Controllers
{
    [ApiController]
    [Route("api/0v1/[controller]")]
    [Authorize(Policy = "Manager")]
    public class InitOrganizationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InitOrganizationController(IMediator mediator)
        {
            _mediator = mediator ??
                throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("InitBusiness/company/{organization}", Name = "InitBusiness")]
        [HttpHead("InitBusiness/company/{organization}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<InitOrganizationResponse>> InitBusiness(string organization,
        [FromHeader(Name = "Authorization")] string? mediaTypeAuthorization)
        {
            var command = new InitOrganizationCommand()
            {
                OrganizationName = organization,
                Token = mediaTypeAuthorization
            };

            var VMresponse = await _mediator.Send(command);

            return VMresponse;
        }

       

        [HttpOptions]
        public IActionResult GetControllerOptions()
        {
            Response.Headers.Add("Allow", "OPTIONS,POST,HEAD");
            return Ok();
        }
    }
}
