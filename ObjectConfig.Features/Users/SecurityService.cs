using ObjectConfig.Data;
using ObjectConfig.Exceptions;
using ObjectConfig.Model;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ObjectConfig.Features.Users
{
    public class SecurityService
    {
        private readonly UserRepository _userRepository;
        private readonly IUserProvider _userProvider;

        private User? _domainUser;

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

        public bool IsGlobalAdminitrator()
        {
            return TryCheckAccess(User.Role.GlobalAdministrator).Result;
        }

        private User MapUser(UserDto tempUser)
        {
            return new User(tempUser.ExternalId, tempUser.DisplayName, tempUser.Email, tempUser.AccessRole);
        }

        public async Task<bool> TryCheckAccess(User.Role minimalAccessLevel)
        {
            var user = await GetCurrentUser();
            return user.AccessRole >= minimalAccessLevel;
        }

        public async Task<bool> CheckAccess(User.Role minimalAccessLevel,
            [CallerMemberName] string callMemeber = "")
        {
            if (!(await TryCheckAccess(minimalAccessLevel)))
            {
                throw new ForbidenException($"Does not have sufficient privileges to perform the operation{DefineNameOperation(callMemeber)}. Operation required '{minimalAccessLevel}' role");
            }
            return true;
        }

        public async Task<TUser> CheckEntityAcces<TRoleEnum, TUser>(IUsers<TUser, TRoleEnum> entity,
            TRoleEnum minimalEntityRole,
            User.Role globalRole = User.Role.GlobalAdministrator,
            [CallerMemberName] string callMemeber = "")
            where TRoleEnum : Enum
            where TUser : IRole<TRoleEnum>
        {
            var user = await GetCurrentUser();
            var userEntity = entity.Users.Single(f => f.UserId == user.UserId);

            if ((userEntity != null && Convert.ToInt32(userEntity.AccessRole) >= Convert.ToInt32(minimalEntityRole))
                || await TryCheckAccess(globalRole))
            {
                return userEntity;
            }

            throw new ForbidenException($"Does not have sufficient privileges to perform the operation{DefineNameOperation(callMemeber)}. Operation required '{minimalEntityRole}' or {globalRole} role");
        }

        private string DefineNameOperation(string memberName)
        {
            if (!string.IsNullOrEmpty(memberName))
            {
                var operationDefinition = System.Text.RegularExpressions.Regex.Replace(memberName, "([A-Z])", " $1").Replace("  ", " ").Trim().ToLower();
                return $": '{operationDefinition}'";
            }
            return string.Empty;
        }
    }
}
