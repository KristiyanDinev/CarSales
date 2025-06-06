using Microsoft.AspNetCore.Identity;

namespace CarSales.Models.Identity
{
    public class IdentityRoleModel : IdentityRole
    {
        public IdentityRoleModel(string roleName) : base(roleName)
        {
            this.Name = roleName;
        }

        public string? Description { get; set; } = "No description provided.";
        public override string? Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
