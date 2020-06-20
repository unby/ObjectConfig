using Microsoft.Extensions.DependencyInjection;

namespace ObjectConfig.Features.Users
{
    public static class UserFeature
    {
        public static IServiceCollection UserFeatureRegister(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<SecurityService>();
            services.AddScoped<IUserProvider, UserProvider>();

            return services;
        }
    }
}
