using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ObjectConfig.Data;
using ObjectConfig.Features.Common;
using ObjectConfig.Features.Environments;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectConfig.Features.Configs
{
    public class ConfigService
    {
        private readonly ObjectConfigContext _objectConfigContext;
        private readonly EnvironmentService _environmentService;

        public ConfigService(ObjectConfigContext objectConfigContext, EnvironmentService environmentService)
        {
            _objectConfigContext = objectConfigContext;
            _environmentService = environmentService;
        }

        public async Task<Config> GetConfig(ConfigArgumentCommand request, CancellationToken cancellationToken)
        {
            var env = await _environmentService.GetEnvironment(request, cancellationToken);

            var result = await GetConfig(env.EnvironmentId, request, cancellationToken);

            request.ThrowNotFoundExceptionWhenValueIsNull(result);

            return result;
        }

        public async Task<Config> GetConfig(ConfigArgumentCommand request, EnvironmentRole environmentRole, CancellationToken cancellationToken)
        {
            var env = await _environmentService.GetEnvironment(request, environmentRole, cancellationToken);

            var result = await GetConfig(env.EnvironmentId, request, cancellationToken);

            request.ThrowNotFoundExceptionWhenValueIsNull(result);

            return result;
        }

        public async Task<Config> GetConfig(int environmentId, ConfigArgumentCommand request, CancellationToken cancellationToken)
        {
            return await _objectConfigContext.Configs.SingleOrDefaultAsync(
                w => w.EnvironmentId.Equals(environmentId) && w.Code == request.ConfigCode && w.DateTo == null &&
                     ((w.VersionFrom <= request.VersionFrom && request.VersionFrom < w.VersionTo) ||
                      (w.VersionFrom <= request.VersionFrom && w.VersionTo == null)), cancellationToken);
        }

        public async Task<string> GetConfigValue(Func<Task<Config>> func, CancellationToken cancellationToken)
        {
            var id = (await func()).ConfigId;
            var k = (await _objectConfigContext.ConfigCache.
                SingleOrDefaultAsync(f => f.ConfigId == id, cancellationToken)).ConfigValue;
            return k;
        }

        public async Task<(Config config, ConfigElement root, ConfigElement[] all)> GetConfigElement(Func<Task<Config>> func, CancellationToken token)
        {
            var config = await func();
            var id = config.ConfigId;

            var all = await  _objectConfigContext.ConfigElements.
                Where(config => config.ConfigId == id && config.DateTo == null).Include(i=>i.TypeElement)
                .Include(i=>i.Value.Where(w=>w.DateTo==null)).ToArrayAsync(token);

            var root = all.First(f => f.ParrentConfigElementId == null);
            return (config, root, all);
        }
    }
}
