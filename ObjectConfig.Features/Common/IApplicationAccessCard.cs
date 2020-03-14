using ObjectConfig.Data;

namespace ObjectConfig.Features.Common
{
    public interface IApplicationAccessCard : IAccessCardOfUser
    {
        string ApplicationCode { get; }
        int ApplicationId { get; }

        ApplicationRole ApplicationRole { get; }
    }
}
