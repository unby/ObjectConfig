using ObjectConfig.Exceptions;

namespace ObjectConfig.Features.Common
{
    public abstract class ConfigArgumentCommand : EnvironmentArgumentCommand
    {
        protected ConfigArgumentCommand(string applicationCode, string environmentCode, string configCode, string? versionFrom, string? versionTo) : base(applicationCode, environmentCode)
        {
            if (string.IsNullOrWhiteSpace(configCode))
            {
                throw new RequestException($"Parameter '{nameof(configCode)}' isn't should empty");
            }

            ConfigCode = configCode;
            VersionFrom = versionFrom;
            VersionTo = versionTo;
        }

        public string ConfigCode { get; }
        public string? VersionFrom { get; }
        public string? VersionTo { get; }
    }
}
