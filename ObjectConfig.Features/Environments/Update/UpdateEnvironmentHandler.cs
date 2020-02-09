using MediatR;
using Microsoft.EntityFrameworkCore;
using ObjectConfig.Data;
using ObjectConfig.Features.Common;
using ObjectConfig.Features.Users;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Environment = ObjectConfig.Data.Environment;

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
                                join users in _configContext.UsersApplications.Where(w => w.UserId.Equals(user.UserId) && w.AccessRole == UsersApplications.Role.Administrator)
                                        on app.ApplicationId equals users.ApplicationId into userApp
                                from appAccess in userApp.DefaultIfEmpty()
                                select new
                                {
                                    app = app,
                                    appAccess
                                }).FirstOrDefaultAsync(cancellationToken);

            request.ThrowNotFoundExceptionWhenValueIsNull(result);

            request.ThrowForbidenExceptionWhenValueIsNull(result.appAccess);

            var environment = new Environment(request.EnvironmentDefinition.Name, request.EnvironmentCode, request.EnvironmentDefinition.Description, result.app);
            var uEnv = new UsersEnvironments(user, environment, UsersEnvironments.Role.Editor);

            _configContext.UsersEnvironments.Add(uEnv);
            await _configContext.SaveChangesAsync(cancellationToken);

            return uEnv;
        }
    }
}
