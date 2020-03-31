using System.Threading;
using System.Threading.Tasks;
using CQRS.Service.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CQRS.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(CreateUserDto request, CancellationToken cancellationToken)
        {
            var result = await _userService.CreateUserAsync(request, cancellationToken);
            if (result.userId > 0)
            {
                return Ok(result.userId);
            }
            else
            {
                return BadRequest(result.errors);
            }
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto request, CancellationToken cancellationToken)
        {
            if (await _userService.ValidateUser(request, cancellationToken))
            {
                var accessToken = await _userService.CreateAccessToken(request, cancellationToken);
                return Ok(accessToken);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}