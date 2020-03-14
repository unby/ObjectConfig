using MediatR;
using Microsoft.EntityFrameworkCore;
using ObjectConfig.Data;
using ObjectConfig.Features.Common;
using ObjectConfig.Features.Environments;
using ObjectConfig.Features.Users;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectConfig.Features.Configs.FindByCode
{
    public class FindConfigHandler : IRequestHandler<FindConfigCommand, Config>
    {
        private readonly SecurityService _securityService;
        private readonly ObjectConfigContext _configContext;
        private readonly EnvironmentService _environmentService;

        public FindConfigHandler(SecurityService securityService, ObjectConfigContext configContext, EnvironmentService environmentService)
        {
            _securityService = securityService;
            _configContext = configContext;
            _environmentService = environmentService;
        }

        public async Task<Config> Handle(FindConfigCommand request, CancellationToken cancellationToken)
        {
            var env = await _environmentService.GetEnvironment(request, cancellationToken);

            var result = await _configContext.Configs.
                SingleOrDefaultAsync(w => w.EnvironmentId.Equals(env.EnvironmentId) && w.Code == request.ConfigCode && w.VersionTo == null, cancellationToken);

            request.ThrowNotFoundExceptionWhenValueIsNull(result);
            
            return result;
        }
    }
}
