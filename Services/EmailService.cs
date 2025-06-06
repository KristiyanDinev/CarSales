using CarSales.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace CarSales.Services
{
    public class EmailService : IEmailSender<IdentityUserModel>
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;

        public Task SendConfirmationLinkAsync(IdentityUserModel user, string email, string confirmationLink)
        {
            return Task.CompletedTask;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.CompletedTask; // Placeholder for actual email sending logic
        }

        public Task SendPasswordResetCodeAsync(IdentityUserModel user, string email, string resetCode)
        {
            return Task.CompletedTask;
        }

        public Task SendPasswordResetLinkAsync(IdentityUserModel user, string email, string resetLink)
        {
            return Task.CompletedTask;
        }
    }
}
