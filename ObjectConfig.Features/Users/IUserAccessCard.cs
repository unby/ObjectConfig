using ObjectConfig.Data;

namespace ObjectConfig.Features.Users
{
    public interface IAccessCardOfUser
    {
        int UserId { get; }
        User.Role UserRole { get; }
    }
}
