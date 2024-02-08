using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multitenant.Application.Contracts.Services;
using Multitenant.Application.CQRS.Commands.CreateUser.Resources;
using Multitenant.Application.CQRS.Commands.CreateUser;
using Multitenant.Application.Models.LoginService;
using Multitenant.Application.CQRS.Commands.CreateOrganization;
using Multitenant.Application.CQRS.Commands.CreateOrganization.Resources;
using Multitenant.Application.Constants;

namespace Multitenant.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class IdentityAdminController : ControllerBase
    {
        private readonly ILogginService _loginService;
        private readonly IMediator _mediator;

        public IdentityAdminController(
        ILogginService loginService,
        IMediator mediator
        )
        {
            _loginService = loginService ??
                throw new ArgumentNullException(nameof(loginService));
            _mediator = mediator ??
              throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("Login/us")]
        [HttpHead("Login/us")]
        [Consumes(//Content-Type
        "application/json")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest request)
        {
            return Ok(await _loginService.LoginUser(request));
        }

        [HttpPost("CreateOrganization")]
        [HttpHead("CreateOrganization")]
        [Consumes(//Content-Type
        "application/json")]
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult<CreateOrganizationResponse>> CreateOrganization([FromBody] CreateOrganizationRequest request,
            [FromHeader(Name = "Authorization")] string? mediaTypeAuthorization)
        {
            var command = new CreateOrganizationCommand() { 
                UserName = request.UserName,
                Password = request.Password,
                Rol = CustomRoles.Organizacion,
                Token = mediaTypeAuthorization,
                OrganizationName = request.OrganizationName
            };
            var VMresponse = await _mediator.Send(command);

            return VMresponse;
        }

        [HttpPost("CreateUser/{organizacion}")]
        [HttpHead("CreateUser/{organizacion}")]
        [Consumes(//Content-Type
        "application/json")]
        [Authorize (Policy = "Organization")]
        public async Task<ActionResult<CreateUserResponse>> CreateUser([FromBody] CreateUserRequest request,string organizacion,
        [FromHeader(Name = "Authorization")] string? mediaTypeAuthorization)
        {

            var command = new CreateUserCommand()
            {
                UserName = request.UserName,
                Password = request.Password,
                Rol = CustomRoles.OrganizacionUsuario,
                Token = mediaTypeAuthorization,
                OrganizationName = organizacion
            };

            var VMresponse = await _mediator.Send(command);

            return VMresponse;
        }


        [HttpOptions]
        public IActionResult GetControllerOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST,PATCH,HEAD");
            return Ok();
        }
    }
}
