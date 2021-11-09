using System;
using Microsoft.AspNetCore.Identity;
using MVPMatch.Application.Interfaces;
using MVPMatch.Application.Models.Identity;
using MVPMatch.Domain.Identity;
using MVPMatch.Infrastructure.Identity.Models;
using MVPMatch.Infrastructure.Persistence;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace MVPMatch.Infrastructure.Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly ITokenValidator _tokenValidator;
        private readonly ITokenParser _tokenParser;

        public IdentityService(
            ApplicationDbContext applicationDbContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ITokenGenerator tokenGenerator,
            ITokenValidator tokenValidator,
            ITokenParser tokenParser)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenGenerator = tokenGenerator;
            _tokenValidator = tokenValidator;
            _tokenParser = tokenParser;

            // TODO: remove seeding later, just for testing
            SeedRoles(new[] { "buyer", "seller" });
        }

        public async Task<AuthenticationResult> CreateUserAsync(CreateUserDto userDto)
        {
            if (await _userManager.FindByEmailAsync(userDto.Email) != null)
            {
                return AuthenticationResult.Failure("User with this email already exists");
            }

            var user = new ApplicationUser
            {
                Email = userDto.Email,
                UserName = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Deposit = default
            };

            var createdUser = await _userManager.CreateAsync(user, userDto.Password);

            // TODO: replace eventually with a proper solution, right now role is assigned based on the email just for the testing purposes.
            if (user.Email.Contains("buy"))
            {
                await _userManager.AddToRoleAsync(user, "buyer");
            }
            if (user.Email.Contains("seller"))
            {
                await _userManager.AddToRoleAsync(user, "seller");
            }

            await _applicationDbContext.SaveChangesAsync();

            return createdUser.Succeeded
                ? await CreateTokenAuthResponse(user, false)
                : AuthenticationResult.Failure(createdUser.Errors.Select(e => e.Description).ToArray());
        }

        public async Task<AuthenticationResult> LoginAsync(LoginUserDto userDto)
        {
            var user = await _userManager.FindByEmailAsync(userDto.Email);

            return user != null && await _userManager.CheckPasswordAsync(user, userDto.Password)
                ? await CreateTokenAuthResponse(user, _applicationDbContext.RefreshTokens.Any(t => t.UserId == user.Id && t.IsValid))
                : AuthenticationResult.Failure("Wrong username or password");
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(RefreshUserDto userDto)
        {
            var principal = _tokenParser.GetPrincipalFromToken(userDto.Token);

            if (principal is not null && await _tokenValidator.IsValid(principal, userDto.RefreshToken))
            {
                var user = await _userManager
                    .FindByIdAsync(principal.Claims.Single(c => c.Type == JwtRegisteredClaimNamesEx.Id).Value);

                return await CreateTokenAuthResponse(user, false);
            }

            return AuthenticationResult.Failure("Invalid token");
        }

        public async Task LogoutAsync(LogoutUserDto userDto)
        {
            await _tokenValidator.Invalidate(userDto.RefreshToken);
        }

        public async Task<decimal> UpdateDeposit(string email, decimal deposit)
        {
            var user = await _userManager.FindByEmailAsync(email);
            user.Deposit = deposit;

            await _userManager.UpdateAsync(user);
            await _applicationDbContext.SaveChangesAsync();
            return user.Deposit;
        }

        private void SeedRoles(IEnumerable<string> roles)
        {
            foreach (var roleName in roles)
            {
                var role = new IdentityRole {Name = roleName, ConcurrencyStamp = Guid.NewGuid().ToString()};
                if (!_roleManager.RoleExistsAsync(roleName).Result)
                {
                    _roleManager.CreateAsync(role);
                }
            }
            
        }

        private async Task<AuthenticationResult> CreateTokenAuthResponse(ApplicationUser user, bool wasAlreadyLoggedIn)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = _tokenGenerator.AccessToken(user, tokenHandler);

            var refreshToken = await _tokenGenerator.RefreshToken(user, token);

            var result = AuthenticationResult.Success(tokenHandler.WriteToken(token), refreshToken.Value);
            result.IsAlreadyLoggedIn = wasAlreadyLoggedIn;

            return result;
        }
    }
}
