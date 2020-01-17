using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ObjectConfig.Data;
using ObjectConfig.Model;
using System;

namespace ObjectConfig
{
    public static class ServiceRegister
    {
        public static IServiceCollection ObjectConfigServices(this IServiceCollection services, Action<DbContextOptionsBuilder> configureDb)
        {
            services.AddScoped<UserRepository>();
            services.AddDbContext<ObjectConfigContext>(configureDb);
            services.AddScoped<ApplicationRepository>();
            services.AddScoped<ConfigRepository>();
            services.AddScoped<ConfigElementRepository>();

            return services;
        }
    }
}
