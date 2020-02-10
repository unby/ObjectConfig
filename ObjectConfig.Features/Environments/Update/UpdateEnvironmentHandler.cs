using MediatR;
using Microsoft.EntityFrameworkCore;
using ObjectConfig.Data;
using ObjectConfig.Features.Common;
using ObjectConfig.Features.Users;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectConfig.Features.Environments.Update
{
    public class UpdateEnvironmentHandler : IRequestHandler<UpdateEnvironmentCommand, UsersEnvironments>
    {
        private readonly SecurityService _securityService;
        private readonly ObjectConfigContext _configContext;

        public UpdateEnvironmentHandler(SecurityService securityService, ObjectConfigContext configContext)
        {
            _securityService = securityService;
            _configContext = configContext;
        }

        public async Task<UsersEnvironments> Handle(UpdateEnvironmentCommand request, CancellationToken cancellationToken)
        {
            var user = await _securityService.GetCurrentUser();

            var result = await (from app in _configContext.Applications.Where(w => w.Code.Equals(request.ApplicationCode))
                                join env in _configContext.Environments.Include(i => i.Users).Where(w => w.Code.Equals(request.EnvironmentCode))
                                        on app.ApplicationId equals env.ApplicationId
                                join appUser in _configContext.UsersApplications.Where(w => w.UserId.Equals(user.UserId) && w.AccessRole == UsersApplications.Role.Administrator)
                                        on app.ApplicationId equals appUser.ApplicationId
                                select env
                                ).FirstOrDefaultAsync(cancellationToken);

            request.ThrowNotFoundExceptionWhenValueIsNull(result);

            if (request.EnvironmentDefinition != null)
            {
                result.Rename(request.EnvironmentDefinition.Name);
                result.NewDefinition(request.EnvironmentDefinition.Description);
            }

            /*if (request.Users != null)
            {
                foreach (var changeUser in request.Users.Value.Where(w => w.UserId != userApplication?.UserId))
                {
                    var foundUser = application.Users.FirstOrDefault(f => f.UserId == changeUser.UserId);
                    if (foundUser != null)
                    {
                        switch (changeUser.Operation)
                        {
                            case EntityOperation.Create:
                                _configContext.UsersApplications.Add(
                                    new UsersApplications(changeUser.UserId, application.ApplicationId, changeUser.Role));
                                break;
                            case EntityOperation.Update:
                                foundUser.AccessRole = changeUser.Role;
                                _configContext.UsersApplications.Update(foundUser);
                                break;
                            case EntityOperation.Delete:
                                _configContext.UsersApplications.Remove(foundUser);
                                break;
                        }
                    }

                }
            }*/

            await _configContext.SaveChangesAsync(cancellationToken);

            return result.Users.FirstOrDefault();
        }
    }
}
