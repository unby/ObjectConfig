using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ObjectConfig;
using ObjectConfig.Data;
using System;
using System.Net.Http;
using Xunit.Abstractions;

namespace UnitTests
{
    public static class TestExtensions
    {
        private static readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings()
        {
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
        };

        public static User Admin(this ObjectConfigContext context)
        {
            return context.Users.Find(Constants.AdminId);
        }

        public static T GetInstance<T>(this IServiceScope scope)
        {
            return scope.ServiceProvider.GetRequiredService<T>();
        }

        public static T GetInstance<T>(this TestServer server)
        {
            return server.Services.GetService<T>();
        }

        public static (T instance, IServiceScope scope) GetInstanceFromScope<T>(this TestServer server)
        {
            var scope = server.Services.CreateScope();
            T instance = scope.ServiceProvider.GetService<T>();
            return (instance, scope);
        }

        public static void WriteLine(this ITestOutputHelper output, object obj)
        {
            output.WriteLine(obj != null ? obj.ToString() : "null");
        }

        public static HttpClient CreateHttpClient(this TestServer server)
        {
            var client = server.CreateClient();
            
            return client;
        }

        public static T Deserialize<T>(this HttpResponseMessage httpResponse)
        {
            var settings = new JsonSerializerSettings
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ContractResolver = new ContractResolverWithPrivates()
            };
            var str = httpResponse.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(str, settings);
        }

        public static HttpContent Serialize<T>(this T obj)
        {
            var stringPayload = JsonConvert.SerializeObject(obj);

            var httpContent = new StringContent(stringPayload, System.Text.Encoding.UTF8, "application/json");
            return httpContent;
        }
    }
}
