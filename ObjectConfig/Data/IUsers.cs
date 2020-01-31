using System;
using System.Collections.Generic;

namespace ObjectConfig.Data
{
    public interface IUsers<TUser, TEnumRole>
        where TEnumRole : Enum
        where TUser : IRole<TEnumRole>
    {
        IList<TUser> Users { get; }
    }
}
