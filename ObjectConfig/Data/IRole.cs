using System;

namespace ObjectConfig.Data
{
    public interface IRole<TEnumLevel> where TEnumLevel : Enum
    {
        int UserId { get; }
        TEnumLevel AccessRole { get; set; }
    }
}
