using Microsoft.EntityFrameworkCore;
using ObjectConfig.Data;
using ObjectConfig.Features.Common;
using ObjectConfig.Features.Environments;
using System;
using System.Linq;
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
            UsersEnvironments env = await _environmentService.GetEnvironment(request, cancellationToken);

            Config result = await GetConfig(env.EnvironmentId, request, cancellationToken);

            request.ThrowNotFoundExceptionWhenValueIsNull(result);

            return result;
        }

        public async Task<Config> GetConfig(ConfigArgumentCommand request, EnvironmentRole environmentRole, CancellationToken cancellationToken)
        {
            Data.Environment env = await _environmentService.GetEnvironment(request, environmentRole, cancellationToken);

            Config result = await GetConfig(env.EnvironmentId, request, cancellationToken);

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

        public async Task<(Config config, ConfigElement root, ConfigElement[] all)> GetConfigElement(Func<Task<Config>> func, CancellationToken token)
        {
            Config config = await func();
            int id = config.ConfigId;

            ConfigElement[] all = await _objectConfigContext.ConfigElements.
                Where(config => config.ConfigId == id && config.DateTo == null).Include(i => i.TypeElement)
                .Include(i => i.Value.Where(w => w.DateTo == null)).ToArrayAsync(token);

            ConfigElement root = all.First(f => f.ParrentConfigElementId == null);
            return (config, root, all);
        }
    }
}
