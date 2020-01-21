using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
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

        public static HttpClient CreateHttpClient(this TestServer server, string subAdress = "feature")
        {
            var client = server.CreateClient();
            client.BaseAddress = new System.Uri(client.BaseAddress.ToString() + subAdress);
            return client;
        }

        public static T Deserialize<T>(this HttpResponseMessage httpResponse)
        {
            return JsonConvert.DeserializeObject<T>(httpResponse.Content.ReadAsStringAsync().Result);
        }

        public static HttpContent Serialize<T>(this T obj) 
        {
            var stringPayload = JsonConvert.SerializeObject(obj);

            var httpContent = new StringContent(stringPayload, System.Text.Encoding.UTF8, "application/json");
            return httpContent;
        }
    }
}
