using ObjectConfig.Data;
using ObjectConfig.Exceptions;
using System;

namespace ObjectConfig.Features.Common
{
    public abstract class ConfigArgumentCommand : EnvironmentArgumentCommand
    {
        protected ConfigArgumentCommand(string applicationCode, string environmentCode, string configCode, string? versionFrom)
            : base(applicationCode, environmentCode)
        {
            if (string.IsNullOrWhiteSpace(configCode))
            {
                throw new RequestException($"Parameter '{nameof(configCode)}' isn't should empty");
            }

            ConfigCode = configCode;

            if (!string.IsNullOrWhiteSpace(versionFrom))
            {
                From = new Version(versionFrom);
                VersionFrom = Config.ConvertVersionToLong(From);
            }
            else
            {
                From = Config._default;
                VersionFrom = Config._majorSection;
            }
        }

        public readonly Version From;

        public long VersionFrom { get; }
        public string ConfigCode { get; }

        public override string ToString()
        {
            return $"Config {ConfigCode}:{From}(env:{EnvironmentCode}-app:{ApplicationCode})";
        }
    }
}
