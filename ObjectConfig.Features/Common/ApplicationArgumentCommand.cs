using ObjectConfig.Exceptions;

namespace ObjectConfig.Features.Common
{
    public abstract class ApplicationArgumentCommand
    {
        protected ApplicationArgumentCommand(string applicationCode)
        {
            if (string.IsNullOrWhiteSpace(applicationCode))
            {
                throw new RequestException($"Parameter '{nameof(applicationCode)}' isn't should empty");
            }

            ApplicationCode = applicationCode;
        }

        public string ApplicationCode { get; }

        public override string ToString()
        {
            return $"Application {ApplicationCode}";
        }
    }
}
