using Microsoft.Extensions.DependencyInjection;

namespace ObjectConfig.Features.Applictaions
{
    public static class ApplicationFeature
    {
        public static IServiceCollection ApplicationFeatureRegister(this IServiceCollection services)
        {
            services.AddScoped<ApplicationService>();

            return services;
        }
    }
}
