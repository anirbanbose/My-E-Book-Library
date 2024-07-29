using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyEbookLibrary.Email.Emails;

namespace MyEbookLibrary.Email
{

    public static class EmailServiceRegistration
    {
        public static IServiceCollection ConfigureEmailServices(this IServiceCollection services, IConfiguration configuration)
        {
            var emailConfig = configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();

            services.AddSingleton(emailConfig);
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IAccountEmails, AccountEmails>();

            return services;
        }
    }
}
