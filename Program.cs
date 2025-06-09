using CarSales.Database;
using CarSales.Models.Identity;
using CarSales.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;

namespace CarSales
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string connectionString = builder.Configuration.GetValue<string>("ConnectionString") ?? throw new Exception("No Connection String provided.");

            builder.Services.AddTransient<IEmailSender<IdentityUserModel>, EmailService>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<DatabaseContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            
            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();

            builder.Services.AddRateLimiter(_ => _
                .AddFixedWindowLimiter(policyName: "fixed", options =>
                {
                    options.PermitLimit = 1;
                    options.Window = TimeSpan.FromSeconds(1);
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = 2;
                })
            );

            builder.Services.AddIdentity<IdentityUserModel, IdentityRoleModel>(
                options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;

                // User settings
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+%";
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

            })
            .AddEntityFrameworkStores<DatabaseContext>()
            .AddDefaultTokenProviders();


            builder.Services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/login";
                options.LogoutPath = "/logout";
                options.AccessDeniedPath = "/login";
            });

            builder.Services.AddSwaggerGen();

            WebApplication app = builder.Build();

            app.MapIdentityApi<IdentityUserModel>();
            app.MapSwagger();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            using (var scope = app.Services.CreateScope())
            {
                UserManager<IdentityUserModel> userManager =
                    scope.ServiceProvider.GetRequiredService<UserManager<IdentityUserModel>>();

                RoleManager<IdentityRoleModel> roleManager =
                    scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRoleModel>>();

                string role = "Admin";
                if (!(await roleManager.RoleExistsAsync(role)))
                {
                    await roleManager.CreateAsync(new IdentityRoleModel { 
                        Name = role,
                        Description = "Gives access to the admin page."
                    });
                }

                IdentityUserModel? user = await userManager.FindByEmailAsync("admin@example.com");
                if (user != null)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
            app.MapControllers();
            app.Run();
        }
    }
}
