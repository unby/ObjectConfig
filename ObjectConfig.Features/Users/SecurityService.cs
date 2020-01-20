using ObjectConfig.Data;
using ObjectConfig.Model;
using System.Threading.Tasks;

namespace ObjectConfig.Features.Users
{
    public class SecurityService
    {
        private readonly UserRepository _userRepository;
        private readonly IUserProvider _userProvider;

        private User _domainUser;

        public SecurityService(UserRepository userRepository, IUserProvider userProvider)
        {
            _userRepository = userRepository;
            _userProvider = userProvider;
        }

        public async Task<User> GetCurrentUser()
        {
            var tempUser = _userProvider.GetCurrentUser();
            if (_domainUser != null)
            {
                return _domainUser;
            }

            _domainUser = await _userRepository.GetUserByExternalId(tempUser.ExternalId);
            if (_domainUser == null)
            {
                _domainUser = await _userRepository.CreateUser(MapUser(tempUser));
            }

            return _domainUser;
        }

        private User MapUser(UserDto tempUser)
        {
            return new User(tempUser.ExternalId, tempUser.DisplayName, tempUser.Email, User.Role.Viewer);
        }
    }
}
