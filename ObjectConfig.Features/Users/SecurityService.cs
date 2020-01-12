using ObjectConfig.Data;
using ObjectConfig.Model;

namespace ObjectConfig.Features.Users
{

    public class SecurityService
    {
        private readonly UserRepository userRepository;
        private readonly IUserProvider userProvider;
        private User domainUser;

        public SecurityService(UserRepository userRepository, IUserProvider userProvider) {
            this.userRepository = userRepository;
            this.userProvider = userProvider;
        }

        public User GetCurrentUser() 
        {
            var tempUser = userProvider.GetCurrentUser();
            if (domainUser != null)
                return domainUser;
            domainUser = userRepository.GetUserByExternalId(tempUser.ExternalId);
            if (domainUser == null)
                domainUser = userRepository.CreateUser(MapUser(tempUser));

            return domainUser;
        }

        private User MapUser(UserDto tempUser)
        {
            return new User(tempUser.ExternalId, tempUser.DisplayName, tempUser.DisplayName, User.Role.Viewer);
        }
    }
}
