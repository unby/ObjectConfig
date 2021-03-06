﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using ObjectConfig.Data;
using ObjectConfig.Features.Common;
using ObjectConfig.Features.Users;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectConfig.Features.Environments.Create
{
    public class CreateEnvironmentHandler : IRequestHandler<CreateEnvironmentsCommand, UsersEnvironments>
    {
        private readonly SecurityService _securityService;
        private readonly ObjectConfigContext _configContext;

        public CreateEnvironmentHandler(SecurityService securityService, ObjectConfigContext configContext)
        {
            _securityService = securityService;
            _configContext = configContext;
        }

        public async Task<UsersEnvironments> Handle(CreateEnvironmentsCommand request, CancellationToken cancellationToken)
        {
            User user = await _securityService.GetCurrentUser();

            var result = await (from app in _configContext.Applications.Where(w => w.Code.Equals(request.ApplicationCode))
                                join users in _configContext.UsersApplications.Where(w => w.UserId.Equals(user.UserId) && w.AccessRole == ApplicationRole.Administrator)
                                        on app.ApplicationId equals users.ApplicationId into userApp
                                from appAccess in userApp.DefaultIfEmpty()
                                select new
                                {
                                    app,
                                    appAccess
                                }).FirstOrDefaultAsync(cancellationToken);

            request.ThrowNotFoundExceptionWhenValueIsNull(result);

            request.ThrowForbidenExceptionWhenValueIsNull(result.appAccess);

            Environment environment = new Environment(request.Name, request.EnvironmentCode, request.Description, result.app);
            UsersEnvironments env = new UsersEnvironments(user, environment, EnvironmentRole.Editor);

            _configContext.UsersEnvironments.Add(env);
            await _configContext.SaveChangesAsync(cancellationToken);

            return env;
        }
    }
}
