using System;

namespace MVPMatch.Infrastructure.Identity.Models
{
    public class JwtConfig
    {
        public string Secret { get; set; }
        public TimeSpan TokenExpiration { get; set; }
        public TimeSpan RefreshTokenExpiration { get; set; }
    }
}
