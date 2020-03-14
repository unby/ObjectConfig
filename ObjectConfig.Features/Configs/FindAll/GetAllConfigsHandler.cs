using MediatR;
using Microsoft.EntityFrameworkCore;
using ObjectConfig.Data;
using ObjectConfig.Features.Environments;
using ObjectConfig.Features.Users;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectConfig.Features.Configs.FindAll
{
    public class GetAllConfigsHandler : IRequestHandler<GetAllConfigsCommand, List<Config>>
    {
        private readonly SecurityService _securityService;
        private readonly ObjectConfigContext _configContext;
        private readonly EnvironmentService _environmentService;

        public GetAllConfigsHandler(SecurityService securityService, ObjectConfigContext configContext, EnvironmentService environmentService)
        {
            _securityService = securityService;
            _configContext = configContext;
            _environmentService = environmentService;
        }

        public async Task<List<Config>> Handle(GetAllConfigsCommand request, CancellationToken cancellationToken)
        {
            var env = await _environmentService.GetEnvironment(request, cancellationToken);

            return await _configContext.Configs.Where(w => w.EnvironmentId.Equals(env.EnvironmentId) && w.VersionTo == null).ToListAsync(cancellationToken);
        }
    }
}
