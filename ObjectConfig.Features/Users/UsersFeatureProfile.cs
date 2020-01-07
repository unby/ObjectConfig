using AutoMapper;
using ObjectConfig.Data;

namespace ObjectConfig.Features.Users
{
    public class UsersFeatureProfile : Profile
    {
        public UsersFeatureProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}
