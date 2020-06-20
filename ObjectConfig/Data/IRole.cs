using System;

namespace ObjectConfig.Data
{
    public interface IRole<TEnumLevel>
        where TEnumLevel : Enum
    {
        int UserId { get; }

        // To Do change private access
        TEnumLevel AccessRole { get; set; }
    }
}
