using ObjectConfig.Exceptions;

namespace ObjectConfig.Features.Common
{
    public abstract class EnvironmentArgumentCommand : ApplicationArgumentCommand
    {
        protected EnvironmentArgumentCommand(string applicationCode, string environmentCode) : base(applicationCode)
        {
            if (string.IsNullOrWhiteSpace(environmentCode))
            {
                throw new RequestException($"Parameter '{nameof(environmentCode)}' isn't should empty");
            }

            EnvironmentCode = environmentCode;
        }

        public string EnvironmentCode { get; }
    }
}
