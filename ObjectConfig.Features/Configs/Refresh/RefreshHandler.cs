using MediatR;
using ObjectConfig.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectConfig.Features.Configs.Refresh
{
    public class RefreshHandler : IRequestHandler<RefreshCommand, string>
    {
        private readonly ConfigService _configService;
        private readonly CacheService _cacheService;

        public RefreshHandler(ConfigService configService, CacheService cacheService)
        {
            _configService = configService;
            _cacheService = cacheService;
        }

        public async Task<string> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            (Config config, ConfigElement root, ConfigElement[] all) = await _configService.GetConfigElement(
                () => _configService.GetConfig(request, EnvironmentRole.Editor, cancellationToken),
                cancellationToken);
            string result = (await new JsonReducer().Parse(root)).ToString();
            await _cacheService.UpdateJsonConfig(config.ConfigId, result, cancellationToken);
            return result;
        }
    }
}