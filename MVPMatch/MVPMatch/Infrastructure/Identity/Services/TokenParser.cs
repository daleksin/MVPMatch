using Microsoft.IdentityModel.Tokens;
using MVPMatch.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MVPMatch.Infrastructure.Identity.Services
{
    public interface ITokenParser
    {
        ClaimsPrincipal GetPrincipalFromToken(string token);
    }

    public class TokenParser : ITokenParser
    {
        private readonly TokenValidationParameters _tokenValidationParameters;

        public TokenParser(TokenValidationParameters tokenValidationParameters)
        {
            _tokenValidationParameters = tokenValidationParameters;
        }
        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = default(ClaimsPrincipal);

            try
            {
                var validatedPrincipal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);

                if (validatedToken is JwtSecurityToken jwtSecurityToken
                    && jwtSecurityToken.Header.Alg.EqualsIgnoreCaseSafe(SecurityAlgorithms.HmacSha256))
                {
                    principal = validatedPrincipal;
                }
            }
            catch
            {
                // ignored
            }

            return principal;
        }
    }
}
