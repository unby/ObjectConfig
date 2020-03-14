using ObjectConfig.Data;
using ObjectConfig.Exceptions;

namespace ObjectConfig.Features.Common
{
    public class EnvironmentAccessCard : ApplicationAccessCard, IEnvironmentAccessCard
    {
        public EnvironmentAccessCard(string environmentCode, int environmentId, EnvironmentRole environmentRole, IApplicationAccessCard applicationAccessCard)
            : this(applicationAccessCard.UserId, applicationAccessCard.UserRole,
                  applicationAccessCard.ApplicationId, applicationAccessCard.ApplicationCode, applicationAccessCard.ApplicationRole,
                  environmentCode, environmentId, environmentRole)
        {
        }

        public EnvironmentAccessCard(int userId, UserRole userRole,
            int applicationId, string applicationCode, ApplicationRole applicationRole,
            string environmentCode, int environmentId, EnvironmentRole environmentRole)
            : base(userId, userRole,
                  applicationId, applicationCode, applicationRole)
        {
            if (string.IsNullOrWhiteSpace(environmentCode))
            {
                throw new OperationException($"{nameof(environmentCode)} must be has value");
            }

            if (environmentId < 1)
            {
                throw new OperationException($"{nameof(environmentId)} must be greater than zero");
            }

            EnvironmentCode = environmentCode;
            EnvironmentId = environmentId;
            EnvironmentRole = environmentRole;
        }

        public string EnvironmentCode { get; }

        public int EnvironmentId { get; }

        public EnvironmentRole EnvironmentRole { get; }
    }
}
