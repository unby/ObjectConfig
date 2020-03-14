using ObjectConfig.Features.Common;
using System;

namespace ObjectConfig.Features.Configs
{
    public abstract class ConfigCommand : EnvironmentArgumentCommand
    {
        protected ConfigCommand(string applicationCode, string environmentCode, string configCode)
                  : base(applicationCode, environmentCode)
        {
            if (string.IsNullOrWhiteSpace(configCode))
            {
                throw new ArgumentException("message", nameof(configCode));
            }

            ConfigCode = configCode;
        }

        public string ConfigCode { get; }
    }

    public class ConfigService
    {
    }
}
