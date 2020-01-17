using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ObjectConfig.Features.Users;
using System;

namespace ObjectConfig.Features
{
    public static class FeaturesComponent
    {
        public static IMvcBuilder ControllersRegister(this IMvcBuilder services)
        {
            services.AddApplicationPart(typeof(FeaturesComponent).Assembly).AddControllersAsServices();
            return services;
        }

        public static IServiceCollection FeaturesRegister(this IServiceCollection services, Action<DbContextOptionsBuilder> configureDb)
        {
            services.ObjectConfigServices(configureDb);

            services.UserFeatureRegister();

            return services;
        }
    }
}
