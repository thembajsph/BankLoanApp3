using Microsoft.AspNetCore.Identity;

namespace BankLoanApp3.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Other properties if needed
    }
}
