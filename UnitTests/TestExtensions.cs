using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using ObjectConfig;
using ObjectConfig.Data;
using System.Net.Http;
using Xunit.Abstractions;

namespace UnitTests
{
    public static class TestExtensions
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

        public static HttpClient CreateHttpClient(this TestServer server)
        {
            var client = server.CreateClient();
            client.BaseAddress = new System.Uri(client.BaseAddress.ToString() + "feature");
            return client;
        }

        public static T Deserialize<T>(this HttpResponseMessage httpResponse)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(httpResponse.Content.ReadAsStringAsync().Result);
        }
    }
}
