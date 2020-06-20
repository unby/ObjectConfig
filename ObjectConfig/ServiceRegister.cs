using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ObjectConfig.Data;
using ObjectConfig.Model;
using System;

namespace ObjectConfig
{
    public static class ServiceRegister
    {
        public static IServiceCollection AddObjectConfigContext(this IServiceCollection services, Action<DbContextOptionsBuilder> configureDb)
        {
            services.AddDbContext<ObjectConfigContext>(configureDb);

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<UserRepository>();
            services.AddScoped<ApplicationRepository>();

            return services;
        }
    }
}
