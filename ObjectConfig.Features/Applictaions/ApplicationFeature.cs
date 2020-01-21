using Microsoft.Extensions.DependencyInjection;
using ObjectConfig.Features.Users;

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
