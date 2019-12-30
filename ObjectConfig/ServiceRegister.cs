using Microsoft.Extensions.DependencyInjection;
using ObjectConfig.Model;
using ObjectConfig.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

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


            return services;
        }
    }
}
