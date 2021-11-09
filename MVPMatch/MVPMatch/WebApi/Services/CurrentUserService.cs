using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MVPMatch.Application.Interfaces;
using MVPMatch.Infrastructure.Identity;
using MVPMatch.Infrastructure.Identity.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MVPMatch.WebApi.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public string UserEmail => _httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        public string UserId => _httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNamesEx.Id);

        public decimal Deposit => _userManager.FindByEmailAsync(UserEmail).Result.Deposit;
    }
}
