using MediatR;
using Microsoft.EntityFrameworkCore;
using ObjectConfig.Data;
using ObjectConfig.Features.Users;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectConfig.Features.Environments.FindAll
{
    public class GetAllEnvironmentsHandler : IRequestHandler<GetAllEnvironmentsCommand, List<UsersEnvironments>>
    {
        private readonly SecurityService _securityService;
        private readonly ObjectConfigContext _configContext;

        public GetAllEnvironmentsHandler(SecurityService securityService, ObjectConfigContext configContext)
        {
            _securityService = securityService;
            _configContext = configContext;
        }

        public async Task<List<UsersEnvironments>> Handle(GetAllEnvironmentsCommand request, CancellationToken cancellationToken)
        {
            AccessCardOfUser card = await _securityService.GetUserCard();

            var result = (from app in _configContext.UsersApplications.Where(w => w.Application.Code.Equals(request.ApplicationCode) && w.UserId.Equals(card.UserId))
                          join env in _configContext.UsersEnvironments.Include(i => i.Environment) on app.ApplicationId equals env.Environment.ApplicationId into userEnv
                          from environmentAccess in userEnv.DefaultIfEmpty()
                          where (environmentAccess.UserId.Equals(card.UserId) && app.AccessRole < ApplicationRole.Administrator) || (!environmentAccess.UserId.Equals(card.UserId) && app.AccessRole >= ApplicationRole.Administrator)
                          select new
                          {
                              appId = app.ApplicationId,
                              env = environmentAccess
                          }).ToList();

            return result.Select(s => s.env).ToList();
        }
    }
}
