using MediatR;
using ObjectConfig.Data;
using ObjectConfig.Exceptions;
using ObjectConfig.Features.Environments;
using System.Threading;
using System.Threading.Tasks;

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
                config = new Config(request.ConfigCode, env, request.VersionFrom);
            }

            var reader = new ObjectConfigReader(config);
            ConfigElement configElemnt = await reader.Parse(request.Data);
            _configContext.ConfigElements.Add(configElemnt);

            await _configContext.SaveChangesAsync();

            return config;
        }
    }
}
