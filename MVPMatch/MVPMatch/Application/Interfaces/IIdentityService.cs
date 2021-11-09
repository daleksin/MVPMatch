using MVPMatch.Application.Models.Identity;
using MVPMatch.Domain.Identity;
using System.Threading.Tasks;

namespace MVPMatch.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> CreateUserAsync(CreateUserDto user);
        Task<AuthenticationResult> LoginAsync(LoginUserDto user);
        Task<AuthenticationResult> RefreshTokenAsync(RefreshUserDto user);
        Task LogoutAsync(LogoutUserDto user);

        Task<decimal> UpdateDeposit(string email, decimal deposit);
    }
}
