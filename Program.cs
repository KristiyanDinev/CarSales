using CarSales.Database;
using CarSales.Models.Identity;
using CarSales.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;

namespace CarSales
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string _key = builder.Configuration.GetValue<string>("JWT:Key") ?? throw new Exception("No JWT Key provided.");
            string _issuer = builder.Configuration.GetValue<string>("JWT:Issuer") ?? throw new Exception("No JWT Issuer provided.");
            string _audience = builder.Configuration.GetValue<string>("JWT:Audience") ?? throw new Exception("No JWT Audience provided.");

            builder.Services.AddTransient<JwtService>(_ =>
                    new JwtService(_key, _issuer, _audience));

            builder.Services.AddTransient<IEmailSender<IdentityUserModel>, EmailService>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<DatabaseContext>(options =>
                options.UseMySQL(builder.Configuration.GetValue<string>("ConnectionString") ?? ""));

            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_key))
                };
            });


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
            })
            .AddEntityFrameworkStores<DatabaseContext>()
            .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/login";
                options.LogoutPath = "/logout";
            });

            builder.Services.AddSwaggerGen();

            WebApplication app = builder.Build();

            app.MapIdentityApi<IdentityUserModel>();
            app.MapSwagger();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            using (var scope = app.Services.CreateScope())
            {
                await SeedRolesAndAdmin(scope.ServiceProvider);
            }

            app.MapControllers();
            app.Run();
        }

        static async Task SeedRolesAndAdmin(IServiceProvider serviceProvider)
        {
            RoleManager<IdentityRoleModel> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRoleModel>>();
            UserManager<IdentityUserModel> userManager = serviceProvider.GetRequiredService<UserManager<IdentityUserModel>>();

            string roleName = "Admin";
            string userEmail = "admin@example.com";
            string userPassword = "Admin123!";

            // Create role if it doesn't exist
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRoleModel(roleName));
            }

            // Create user if it doesn't exist
            IdentityUserModel? user = await userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                user = new IdentityUserModel
                {
                    UserName = userEmail,
                    Email = userEmail,
                    EmailConfirmed = true
                };

                IdentityResult result = await userManager.CreateAsync(user, userPassword);
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

            // Add user to role if not already added
            if (!await userManager.IsInRoleAsync(user, roleName))
            {
                await userManager.AddToRoleAsync(user, roleName);
            }
        }
    }
}
