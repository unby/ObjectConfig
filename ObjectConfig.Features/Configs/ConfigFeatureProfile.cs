using AutoMapper;
using ObjectConfig.Data;

namespace ObjectConfig.Features.Configs
{
    public class ConfigFeatureProfile : Profile
    {
        public ConfigFeatureProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Config, ConfigDto>();
            CreateMap<ConfigDto, Config>();
        }
    }
}
