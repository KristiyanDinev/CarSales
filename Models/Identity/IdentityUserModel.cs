using Microsoft.AspNetCore.Identity;

namespace CarSales.Models.Identity
{
    public class IdentityUserModel : IdentityUser
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
