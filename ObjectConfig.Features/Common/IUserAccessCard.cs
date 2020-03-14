using ObjectConfig.Data;

namespace ObjectConfig.Features.Common
{
    public interface IAccessCardOfUser
    {
        int UserId { get; }
        UserRole UserRole { get; }
    }
}
