using Microsoft.Extensions.DependencyInjection;

namespace ObjectConfig.FeaturesExtensible
{
    public static class FeaturesExtensibleComponent
    {
        public static IServiceCollection FeaturesExtensibleRegister(this IServiceCollection services)
        {
            // override your components

            return services;
        }
    }
}
