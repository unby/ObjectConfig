using ObjectConfig.Data;
using Xunit.Abstractions;

namespace UnitTests
{
    public static class Extention
    {
        public static User Admin(this ObjectConfigContext context)
        {
            return context.Users.Find(1);
        }

        public static void WriteLine(this ITestOutputHelper output, object obj) 
        { 
            output.WriteLine(obj != null ? obj.ToString() : "null"); 
        }
    }
}
