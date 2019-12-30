using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using ObjectConfig.Model;
using ObjectConfig.Data;
using ObjectConfig;

namespace UnitTests
{
    public class ApplictionRepositoryTest : BaseTest
    {
        private string appCode;
        private string envCode;
        private string configCode;

        public ApplictionRepositoryTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async void CreateApp()
        {
            appCode = Utils.GetStr;
            envCode = Utils.GetStr;
            configCode = Utils.GetStr;
            using (var scope = GetScope())
            {
                var rep = scope.GetInstance<ApplicationRepository>();

                Assert.NotNull(rep);

                rep.Create(ObjectMaker.CreateApplication(s =>
                {
                    s.Code = appCode;
                    s.Name = Utils.GetStr;
                    s.Environments.Add(ObjectMaker.CreateEnvironment(e => { e.Code = envCode; e.Name = Utils.GetStr; }));
                })).Wait();

                var app = await rep.Find(appCode);
                Assert.NotNull(app);
                Assert.Equal(app.Environments[0].Code, envCode);


                var configRepository = scope.GetInstance<ConfigRepository>();
                var config = ObjectMaker.CreateConfig(f =>
                {
                      f.EnvironmentId = app.Environments[0].EnvironmentId;
                    f.Code = configCode;
                });

                await new ObjectConfigReader(config).Parse(Data);
                
                config = configRepository.CreateConfig(config);

                var configFromStore = await configRepository.Find(configCode);


                Assert.NotNull(configFromStore);
                
                Assert.NotEqual(configFromStore, config);
            }
        }

        static string Json =
@"{
  ""Default"": ""Information"",
  ""Microsoft"": ""Warning"",
  ""Microsoft.Hosting.Lifetime"": ""Information""
}";
        static string Data = @"{
  ""Logging"": {
    ""LogLevel"": {
      ""Default"": ""Information"",
      ""Microsoft"": ""Warning"",
      ""Microsoft.Hosting.Lifetime"": ""Information""
    },
    ""GlobalLevel"": true,
    ""Array"": [
    ""123"",
    ""sto""
]
  },
 ""GlobalLevel"": true,
""ComplexArray"":[
{pr1:""key1"", pr2: 500 , pr3:""not default""},
{pr1:""key2"", pr2: -200}
],
  ""ConnectionString"": ""host=trst;instance=stockDB"",
  ""AllowedHosts"": ""*""
}
";

    }
}
/*,
""Array"": [
""123"",
""sto""
]
*/
