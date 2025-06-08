using Microsoft.AspNetCore.Identity;

namespace CarSales.Models.Identity
{
    public class IdentityRoleModel : IdentityRole
    {

        public string? Description { get; set; } = "No description provided.";
        public override string? Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public IdentityRoleModel() : base()
        {
            Name = "Default";
        }

        public IdentityRoleModel(string roleName) : base(roleName)
        {
            this.Name = roleName;
        }

    }
}
