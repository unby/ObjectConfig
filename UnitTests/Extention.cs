using ObjectConfig.Data;
using Xunit.Abstractions;
using ObjectConfig;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTests
{
    public static class Extention
    {
        public static User Admin(this ObjectConfigContext context)
        {
            return context.Users.Find(Constants.AdminId);
        }

        public static T GetInstance<T>(this IServiceScope scope)
        {
            return scope.ServiceProvider.GetRequiredService<T>();
        }
        
        public static void WriteLine(this ITestOutputHelper output, object obj) 
        { 
            output.WriteLine(obj != null ? obj.ToString() : "null"); 
        }
    }
}
