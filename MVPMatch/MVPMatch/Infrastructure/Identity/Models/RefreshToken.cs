using System;

namespace MVPMatch.Infrastructure.Identity.Models
{
    public class RefreshToken
    {
        public int RefreshTokenId { get; set; }

        public string Value { get; set; }

        public string JwtId { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string UserId { get; set; }

        public bool IsValid { get; set; }

        public ApplicationUser User { get; set; }
    }
}
