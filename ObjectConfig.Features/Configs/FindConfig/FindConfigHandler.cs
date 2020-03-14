using MediatR;
using Microsoft.EntityFrameworkCore;
using ObjectConfig.Data;
using ObjectConfig.Features.Common;
using ObjectConfig.Features.Environments;
using ObjectConfig.Features.Users;
using System.Threading;
using System.Threading.Tasks;
using ObjectConfig.Features.Configs.FindConfig;

namespace ObjectConfig.Features.Configs.FindByCode
{
    public class FindConfigHandler : IRequestHandler<FindConfigCommand, Config>
    {
        private readonly ObjectConfigContext _configContext;
        private readonly EnvironmentService _environmentService;

        public FindConfigHandler(ObjectConfigContext configContext, EnvironmentService environmentService)
        {
            _configContext = configContext;
            _environmentService = environmentService;
        }

        public async Task<Config> Handle(FindConfigCommand request, CancellationToken cancellationToken)
        {
            var env = await _environmentService.GetEnvironment(request, cancellationToken);

            var result = await _configContext.GetConfig(env.EnvironmentId, request.ConfigCode, request.VersionFrom, cancellationToken);

            request.ThrowNotFoundExceptionWhenValueIsNull(result);

            return result;
        }
    }
}
