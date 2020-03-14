using ObjectConfig.Data;
using ObjectConfig.Exceptions;
using ObjectConfig.Features.Users;

namespace ObjectConfig.Features.Common
{
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
