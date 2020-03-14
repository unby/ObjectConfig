using Microsoft.EntityFrameworkCore;
using ObjectConfig.Data;
using ObjectConfig.Features.Common;
using ObjectConfig.Features.Users;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectConfig.Features.Environments
{
    public class EnvironmentService
    {
        private readonly SecurityService _securityService;
        private readonly ObjectConfigContext _configContext;

        public EnvironmentService(SecurityService securityService, ObjectConfigContext configContext)
        {
            _securityService = securityService;
            _configContext = configContext;
        }

        public async Task<UsersEnvironments> GetEnvironment(EnvironmentArgumentCommand request, CancellationToken cancellationToken)
        {
            var card = await _securityService.GetUserCard();
            var result = await (from app in _configContext.UsersApplications.Include(i => i.Application).Where(w => w.Application.Code.Equals(request.ApplicationCode) && w.UserId.Equals(card.UserId))
                                join env in _configContext.UsersEnvironments.Include(i => i.Environment) on app.ApplicationId equals env.Environment.ApplicationId into userEnv
                                from environmentAccess in userEnv.DefaultIfEmpty()
                                where (environmentAccess.Environment.Code.Equals(request.EnvironmentCode) && environmentAccess.UserId.Equals(card.UserId))
                                select new
                                {
                                    appId = app.ApplicationId,
                                    env = environmentAccess
                                }).FirstOrDefaultAsync(cancellationToken);

            request.ThrowNotFoundExceptionWhenValueIsNull(result);

            return result.env;
        }

        public async Task<Environment> GetEnvironment(EnvironmentArgumentCommand request, EnvironmentRole requiredRole, CancellationToken cancellationToken) 
        {
            var env = await GetEnvironment(request, cancellationToken);

            await _securityService.CheckEntityAcces(env.Environment, requiredRole);

            return env.Environment;
        }
    }
}
