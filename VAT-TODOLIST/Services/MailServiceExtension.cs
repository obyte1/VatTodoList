using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VAT_TODOLIST.Models;

namespace VAT_TODOLIST.Services
{
    public static class MailServiceExtension
    {
        public static void ConfigureMailService(this IServiceCollection services, IConfiguration Configuration)
        {
            //EmailService registration
            var emailConfig = Configuration
               .GetSection("EmailConfiguration")
               .Get<EmailServerConfig>();
            services.AddSingleton(emailConfig);

        }
    }
}
