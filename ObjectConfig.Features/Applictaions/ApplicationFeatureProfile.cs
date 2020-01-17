using AutoMapper;
using ObjectConfig.Data;

namespace ObjectConfig.Features.Applictaions
{
    public class ApplicationFeatureProfile : Profile
    {
        public ApplicationFeatureProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<UsersApplications, ApplicationDTO>();
            CreateMap<ApplicationDTO, UsersApplications>();
        }
    }
}
