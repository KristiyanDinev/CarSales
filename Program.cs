using CarSales.Database;
using CarSales.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarSales
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<DatabaseContext>(options =>
                options.UseMySQL(
                    builder.Configuration
                    .GetConnectionString("DefaultConnection") ??
                    ""));

            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication();

            builder.Services.AddIdentity<IdentityUserModel, IdentityRoleModel>();

            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.MapIdentityApi<IdentityUserModel>();
            app.MapSwagger();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.MapControllers();
            app.Run();
        }
    }
}
