namespace ObjectConfig.Features.Users
{
    public interface IUserProvider 
    {
        UserDto GetCurrentUser();
    }
}
