using Microsoft.AspNetCore.Mvc;
using DemoAuth.Application.Models.LoginService;
using DsAlpha.RedisStream.Infraestructure.DsAlpha.RedisStream.Infrastructure;

namespace DemoAuth.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PruebaHangfireController : ControllerBase
    {
        private readonly IRedisStreamService _redisStreamService;
        public PruebaHangfireController(
        IRedisStreamService redisStreamService
        )
        {
            _redisStreamService = redisStreamService ??
                throw new ArgumentNullException(nameof(redisStreamService));
        }

        [HttpPost("hangfire/us")]
        [HttpHead("hangfire/us")]
        [Consumes(//Content-Type
        "application/json")]
        public async Task<ActionResult<AuthResponse>> Hangfire([FromBody] AuthRequest request)
        {

            await _redisStreamService.AddToStreamAsync(request);

            return Ok();
        }

       
        [HttpOptions]
        public IActionResult GetControllerOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST,PATCH,HEAD");
            return Ok();
        }
    }
}
