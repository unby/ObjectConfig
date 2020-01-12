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
                var app = new Application(Utils.GetStr, appCode, null);
                app.Environments.Add(new Environment() { Code = envCode, Name = Utils.GetStr });
              await rep.Create(app);

              app = await rep.Find(appCode);
              Assert.NotNull(app);
              Assert.Equal(app.Environments[0].Code, envCode);


                var configRepository = scope.GetInstance<ConfigRepository>();

                var config = new Config("test", new System.Version(1, 0), app.Environments[0].EnvironmentId, null);
                configRepository.CreateConfig(config);

                var  configc = await scope.GetInstance<ConfigElementRepository>().Create(await new ObjectConfigReader(config).Parse(Data));
                Assert.NotEqual(0, configc.ConfigElementId);
                //     
                //     config = configRepository.CreateConfig(config);
                //
                //     var configFromStore = await configRepository.Find(configCode);
                //
                //
                //     Assert.NotNull(configFromStore);
                //     
                //     Assert.NotEqual(configFromStore, config);
            }
        }

        [Fact]
        public async void DeleteConfig()
        {
            appCode = Utils.GetStr;
            envCode = Utils.GetStr;
            configCode = Utils.GetStr;
            using (var scope = GetScope())
            {
                var rep = scope.GetInstance<ConfigElementRepository>();

               var conf=rep.GetConfigElement(3);
                Log.WriteLine(conf);
              //  context.ConfigElement.Remove(configElement);
            //    context.SaveChanges();
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
