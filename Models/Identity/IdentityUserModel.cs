using Microsoft.AspNetCore.Identity;

namespace CarSales.Models.Identity
{
    public class IdentityUserModel : IdentityUser
    {
        public override string? UserName { get; set; }
        public override string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
