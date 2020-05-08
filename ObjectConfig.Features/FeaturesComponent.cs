using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ObjectConfig.Features.Applictaions;
using ObjectConfig.Features.Configs;
using ObjectConfig.Features.Environments;
using ObjectConfig.Features.Users;
using ObjectConfig.Model;

namespace ObjectConfig.Features
{
    public static class FeaturesComponent
    {
        public static IMvcBuilder ControllersRegister(this IMvcBuilder services)
        {
            services.AddApplicationPart(typeof(FeaturesComponent).Assembly).AddControllersAsServices();
            return services;
        }

        public static IServiceCollection FeaturesRegister(this IServiceCollection services)
        {
            services.AddRepositories();

            services.UserFeatureRegister();
            services.ApplicationFeatureRegister();

            services.AddAutoMapper(typeof(FeaturesComponent).Assembly);
            services.AddMediatR(typeof(FeaturesComponent).Assembly);
            services.AddScoped<EnvironmentService>();
            services.AddScoped<ConfigService>();
            services.AddScoped<CacheService>();

            // services.AddScoped<ConfigElementRepository>();
            return services;
        }
    }
}
