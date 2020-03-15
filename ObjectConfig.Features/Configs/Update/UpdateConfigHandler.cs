using MediatR;
using Microsoft.EntityFrameworkCore;
using ObjectConfig.Data;
using ObjectConfig.Features.Common;
using ObjectConfig.Features.Users;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectConfig.Features.Configs.Update
{
    public class UpdateConfigHandler : IRequestHandler<UpdateConfigCommand, Config>
    {
        private readonly SecurityService _securityService;
        private readonly ObjectConfigContext _configContext;

        public UpdateConfigHandler(SecurityService securityService, ObjectConfigContext configContext)
        {
            _securityService = securityService;
            _configContext = configContext;
        }

        public async Task<Config> Handle(UpdateConfigCommand request, CancellationToken cancellationToken)
        {
            var user = await _securityService.GetCurrentUser();

            var result = await (from app in _configContext.Applications.Where(w => w.Code.Equals(request.ApplicationCode))
                                join env in _configContext.Environments.Include(i => i.Users).Where(w => w.Code.Equals(request.EnvironmentCode))
                                        on app.ApplicationId equals env.ApplicationId
                                join appUser in _configContext.UsersApplications.Where(w => w.UserId.Equals(user.UserId) && w.AccessRole == ApplicationRole.Administrator)
                                        on app.ApplicationId equals appUser.ApplicationId
                                select env
                                ).FirstOrDefaultAsync(cancellationToken);

            request.ThrowNotFoundExceptionWhenValueIsNull(result);

            await _configContext.SaveChangesAsync(cancellationToken);

            throw new NotImplementedException();
        }
    }
}
