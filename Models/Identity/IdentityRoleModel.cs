using Microsoft.AspNetCore.Identity;

namespace CarSales.Models.Identity
{
    public class IdentityRoleModel : IdentityRole
    {

        public required string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public IdentityRoleModel() : base("Default")
        {
            Name = "Default";
        }

        public IdentityRoleModel(string roleName) : base(roleName)
        {
            Description = "No description provided.";
        }
    }
}
