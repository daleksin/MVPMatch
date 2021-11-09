using Microsoft.AspNetCore.Identity;

namespace MVPMatch.Infrastructure.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Deposit { get; set; }
    }
}
