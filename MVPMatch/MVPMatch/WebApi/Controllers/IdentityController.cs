using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVPMatch.Application.Interfaces;
using MVPMatch.Application.Models.Identity;
using MVPMatch.Domain.Identity;
using System.Threading.Tasks;

namespace MVPMatch.WebApi.Controllers
{
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost]
        [Route("auth/register")]
        public async Task<ActionResult<AuthenticationResult>> CreateUser([FromBody] CreateUserDto user)
        {
            return await _identityService.CreateUserAsync(user);
        }

        [HttpPost]
        [Route("auth/login")]
        public async Task<ActionResult<AuthenticationResult>> Login([FromBody] LoginUserDto user)
        {
            return await _identityService.LoginAsync(user);
        }


        [HttpPost]
        [Route("auth/refresh")]
        public async Task<ActionResult<AuthenticationResult>> Refresh([FromBody] RefreshUserDto user)
        {
            return await _identityService.RefreshTokenAsync(user);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("auth/logout")]
        public async Task<ActionResult> Logout([FromBody] LogoutUserDto user)
        {
            await _identityService.LogoutAsync(user);
            return Unauthorized();
        }
    }
}
