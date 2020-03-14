using ObjectConfig.Exceptions;

namespace ObjectConfig.Features.Common
{
    public abstract class ConfigArgumentCommand : EnvironmentArgumentCommand
    {
        protected ConfigArgumentCommand(string applicationCode, string environmentCode, string configCode)
            : base(applicationCode, environmentCode)
        {
            if (string.IsNullOrWhiteSpace(configCode))
            {
                throw new RequestException($"Parameter '{nameof(configCode)}' isn't should empty");
            }

            ConfigCode = configCode;
        }

        public string ConfigCode { get; }
    }
}
