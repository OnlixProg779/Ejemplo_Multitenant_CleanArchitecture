using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DemoAuth.Application.Contracts.Services;
using DemoAuth.Application.Models.LoginService;
using DemoAuth.Application.CQRS.Identity.Commands.CreateOrganization.Resources;
using DemoAuth.Application.CQRS.Identity.Commands.CreateUser.Resources;
using DemoAuth.Application.CQRS.Identity.Commands.CreateOrganization;
using DemoAuth.Application.CQRS.Identity.Commands.CreateUser;
using Base.Application.Constants;

namespace DemoAuth.Controllers
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

        [HttpPost("CreateUser/company/{organization}")]
        [HttpHead("CreateUser/company/{organization}")]
        [Consumes(//Content-Type
        "application/json")]
        [Authorize (Policy = "Organization")]
        public async Task<ActionResult<CreateUserResponse>> CreateUser([FromBody] CreateUserRequest request,string organization,
        [FromHeader(Name = "Authorization")] string? mediaTypeAuthorization)
        {

            var command = new CreateUserCommand()
            {
                UserName = request.UserName,
                Password = request.Password,
                Rol = CustomRoles.OrganizacionUsuario,
                Token = mediaTypeAuthorization,
                OrganizationName = organization
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
