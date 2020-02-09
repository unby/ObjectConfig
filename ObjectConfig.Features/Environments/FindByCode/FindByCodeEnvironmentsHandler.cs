using MediatR;
using Microsoft.EntityFrameworkCore;
using ObjectConfig.Data;
using ObjectConfig.Features.Common;
using ObjectConfig.Features.Users;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectConfig.Features.Environments.FindByCode
{
    public class FindByCodeEnvironmentsHandler : IRequestHandler<FindByCodeEnvironmentCommand, UsersEnvironments>
    {
        private readonly SecurityService _securityService;
        private readonly ObjectConfigContext _configContext;

        public FindByCodeEnvironmentsHandler(SecurityService securityService, ObjectConfigContext configContext)
        {
            _securityService = securityService;
            _configContext = configContext;
        }

        public async Task<UsersEnvironments> Handle(FindByCodeEnvironmentCommand request, CancellationToken cancellationToken)
        {
            var card = await _securityService.GetUserCard();

            /*  var result = await (from app in _configContext.UsersApplications.Where(w => w.Application.Code.Equals(request.ApplicationCode) && w.UserId.Equals(card.UserId))
                                  join env in _configContext.UsersEnvironments.Include(i => i.Environment) on app.ApplicationId equals env.Environment.ApplicationId into userEnv
                                  from environmentAccess in userEnv.DefaultIfEmpty()
                                  where (
                                  environmentAccess.Environment.Code.Equals(request.EnvironmentCode)
                                  && (
                                  (environmentAccess.UserId.Equals(card.UserId) && app.AccessRole < UsersApplications.Role.Administrator)
                                  || (!environmentAccess.UserId.Equals(card.UserId) && app.AccessRole >= UsersApplications.Role.Administrator))
                                  )
                                  select new
                                  {
                                      appId = app.ApplicationId,
                                      env = environmentAccess
                                  }).FirstOrDefaultAsync(cancellationToken);
  */
            var result = await (from app in _configContext.UsersApplications.Include(i => i.Application).Where(w => w.Application.Code.Equals(request.ApplicationCode) && w.UserId.Equals(card.UserId))
                                join env in _configContext.UsersEnvironments.Include(i => i.Environment) on app.ApplicationId equals env.Environment.ApplicationId into userEnv
                                from environmentAccess in userEnv.DefaultIfEmpty()
                                where (environmentAccess.Environment.Code.Equals(request.EnvironmentCode) && environmentAccess.UserId.Equals(card.UserId))
                                select new
                                {
                                    appId = app.ApplicationId,
                                    env = environmentAccess
                                }).FirstOrDefaultAsync(cancellationToken);
            /*  System.Console.WriteLine(result2);
              var result = await (from app in _configContext.UsersApplications.Include(i => i.Application).Where(w => w.Application.Code.Equals(request.ApplicationCode) && w.UserId.Equals(card.UserId))
                                  join env in _configContext.UsersEnvironments.Include(i => i.Environment) on app.ApplicationId equals env.Environment.ApplicationId into userEnv
                                  from environmentAccess in userEnv.DefaultIfEmpty()
                                  where (
                                  environmentAccess.Environment.Code.Equals(request.EnvironmentCode)
                                  && (
                                  (environmentAccess.UserId.Equals(card.UserId) && app.AccessRole < UsersApplications.Role.Administrator)
                                  || (!environmentAccess.UserId.Equals(card.UserId) && app.AccessRole >= UsersApplications.Role.Administrator))
                                  )
                                  select new
                                  {
                                      appId = app.ApplicationId,
                                      env = environmentAccess
                                  }).FirstOrDefaultAsync(cancellationToken);
                                  */
            request.ThrowNotFoundExceptionWhenValueIsNull(result);

            return result.env;
        }
    }
}
