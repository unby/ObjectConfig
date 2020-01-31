using System;

namespace ObjectConfig.Features.Users
{
    public interface IUserAcessLevel<TEnumLevel> where TEnumLevel : Enum
    {
        public int UserId { get; }

        public TEnumLevel Role { get; }

        public EntityOperation Operation { get; }
    }
    public enum EntityOperation { Create, Update, Delete }
}
