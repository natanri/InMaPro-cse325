using Microsoft.AspNetCore.Identity;

namespace InMaPro_cse325.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    }
}