using CarSales.Models.Identity;

namespace CarSales.Models
{
    public class UserViewAdminModel
    {
        public required IdentityUserModel User { get; set; }
        public required bool IsAdmin { get; set; }
    }
}
