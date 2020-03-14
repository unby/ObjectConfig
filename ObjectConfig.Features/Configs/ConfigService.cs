using Microsoft.EntityFrameworkCore;
using ObjectConfig.Data;
using ObjectConfig.Features.Common;
using ObjectConfig.Features.Users;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectConfig.Features.Configs
{
    public static class ConfigService
    {

        public static async Task<Config> GetConfig(this ObjectConfigContext context, int environmentId, string configCode, long versionFrom, CancellationToken cancellationToken)
        {
            var result = await context.Configs.
                 SingleOrDefaultAsync(w => w.EnvironmentId.Equals(environmentId) && w.Code == configCode && w.DateTo == null
                 && ((w.VersionFrom <= versionFrom && versionFrom < w.VersionTo) || (w.VersionFrom <= versionFrom && w.VersionTo == null)), cancellationToken);

            return result;
        }
        /*  var result = from current in context.Configs.
                 Where(w => w.EnvironmentId.Equals(environmentId) && w.Code == configCode && w.DateTo == null
                 && ((w.VersionFrom >= versionFrom && versionFrom < w.VersionTo) || (w.VersionFrom >= versionFrom && w.VersionTo == null)))
                         join next in context.Configs.
                         Where(w => w.EnvironmentId.Equals(environmentId) && w.Code == configCode && w.DateTo == null
                         && ((w.VersionFrom >= versionFrom && versionFrom < w.VersionTo) || (w.VersionFrom >= versionFrom && w.VersionTo == null))) on current.VersionFrom equals next.VersionTo into nextConf
                         from next1 in nextConf.DefaultIfEmpty()*/
    }
}
