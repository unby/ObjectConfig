using ObjectConfig.Data;

namespace ObjectConfig.Features.Users
{
    public interface IAccessCardOfUser
    {
        int UserId { get; }
        UserRole UserRole { get; }
    }
}
