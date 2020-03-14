using ObjectConfig.Data;

namespace ObjectConfig.Features.Common
{
    public interface IEnvironmentAccessCard : IApplicationAccessCard
    {
        string EnvironmentCode { get; }

        int EnvironmentId { get; }

        EnvironmentRole EnvironmentRole { get; }
    }
}
