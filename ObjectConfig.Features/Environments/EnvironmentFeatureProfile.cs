using AutoMapper;
using ObjectConfig.Data;

namespace ObjectConfig.Features.Environments
{
    public class EnvironmentFeatureProfile : Profile
    {
        public EnvironmentFeatureProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<UsersEnvironments, EnvironmentDto>();
            CreateMap<EnvironmentDto, UsersEnvironments>();
        }
    }
}
