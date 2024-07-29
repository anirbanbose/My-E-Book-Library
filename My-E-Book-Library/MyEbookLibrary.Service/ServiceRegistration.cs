using Microsoft.Extensions.DependencyInjection;
using MyEbookLibrary.Service.Contracts;
using MyEbookLibrary.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Service
{
    public static class ServiceRegistration
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<IPublisherService, PublisherService>();
            services.AddScoped<IAuthorTypeService, AuthorTypeService>();
            services.AddScoped<IDashboardService, DashboardService>();
            return services;
        }
    }
}
