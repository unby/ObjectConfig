using System.Linq;
using MediatR;
using ObjectConfig.Data;
using ObjectConfig.Exceptions;
using ObjectConfig.Features.Environments;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ObjectConfig.Features.Configs.Create
{
    public class CreateConfigHandler : IRequestHandler<CreateConfigCommand, Config>
    {
        private readonly EnvironmentService _environmentService;
        private readonly ObjectConfigContext _configContext;
        private readonly ConfigService _configService;

        public CreateConfigHandler(EnvironmentService environmentService, ObjectConfigContext configContext, ConfigService configService)
        {
            _environmentService = environmentService;
            _configContext = configContext;
            _configService = configService;
        }

        public async Task<Config> Handle(CreateConfigCommand request, CancellationToken cancellationToken)
        {
            var env = await _environmentService.GetEnvironment(request, EnvironmentRole.Editor, cancellationToken);

            var existConfig = await _configService.GetConfig(env.EnvironmentId, request, cancellationToken);

            if (existConfig != null && existConfig.VersionFrom == request.VersionFrom)
            {
                throw new EntityException($"{request} is exists");
            }

            Config config;
            if (existConfig != null && existConfig.VersionTo == null)
            {
                config = new Config(request.ConfigCode, env, request.VersionFrom);
                existConfig.SetVersionTo(request.From);
            }
            else if (existConfig != null && existConfig.VersionTo != null)
            {
                config = new Config(request.ConfigCode, env, request.VersionFrom, existConfig.VersionTo);
                existConfig.SetVersionTo(request.From);
            }
            else
            {
                var minVer = (await _configContext.Configs
                    .Where(w => w.EnvironmentId.Equals(env.EnvironmentId) && w.Code == request.ConfigCode)
                    .GroupBy(e => 1)
                    .Select(s => s.Min(m => m.VersionFrom)).ToListAsync(cancellationToken))[0];

                if (minVer != 0 && minVer < request.VersionFrom)
                    throw new OperationException(
                        "Incorrect config version in store, VersionTo must be null. Contact your system administrator");

                config = new Config(request.ConfigCode, env, request.VersionFrom, minVer);
            }

            var reader = new ObjectConfigReader(config);
            ConfigElement configElemnt = await reader.Parse(request.Data);
            _configContext.ConfigElements.Add(configElemnt);

            await _configContext.SaveChangesAsync(cancellationToken);

            return config;
        }
    }
}
