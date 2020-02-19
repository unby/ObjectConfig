using ObjectConfig.Data;
using ObjectConfig.Exceptions;
using ObjectConfig.Features.Users;

namespace ObjectConfig.Features.Environments
{
    public class EnvironmentService
    {

    }



    public interface IApplicationAccessCard : IAccessCardOfUser
    {
        string ApplicationCode { get; }
        int ApplicationId { get; }

        ApplicationRole ApplicationRole { get; }
    }

    public interface IEnvironmentAccessCard : IApplicationAccessCard
    {
        string EnvironmentCode { get; }

        int EnvironmentId { get; }

        EnvironmentRole EnvironmentRole { get; }
    }

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

    public class ApplicationAccessCard : AccessCardOfUser, IApplicationAccessCard
    {
        public ApplicationAccessCard(int userId, UserRole userRole, int applicationId, string applicationCode, ApplicationRole applicationRole)
            : base(userId, userRole)
        {
            if (string.IsNullOrWhiteSpace(applicationCode))
            {
                throw new OperationException($"{nameof(applicationCode)} must be has value");
            }
            if (applicationId < 1)
            {
                throw new OperationException($"{nameof(applicationId)} must be greater than zero");
            }

            ApplicationId = applicationId;
            ApplicationCode = applicationCode;
            ApplicationRole = applicationRole;
        }

        public ApplicationAccessCard(int applicationId, string applicationCode, ApplicationRole applicationRole, IAccessCardOfUser accessCardOfUser)
            : this(accessCardOfUser.UserId, accessCardOfUser.UserRole, applicationId, applicationCode, applicationRole)
        {

        }

        public string ApplicationCode { get; }

        public int ApplicationId { get; }

        public ApplicationRole ApplicationRole { get; }
    }
}
