using ObjectConfig;
using ObjectConfig.Data;
using ObjectConfig.Model;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests
{

    public class ApplictionRepositoryTest : BaseTest
    {
        private string _appCode;
        private string _envCode;
        private string _configCode;

        public ApplictionRepositoryTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async void CreateApp()
        {
            _appCode = Utils.GetStr;
            _envCode = Utils.GetStr;
            _configCode = Utils.GetStr;
            using var scope = GetScope();
            var rep = scope.GetInstance<ApplicationRepository>();

            Assert.NotNull(rep);
            var app = new Application(Utils.GetStr, _appCode, null);
            app.Environments.Add(new Environment() { Code = _envCode, Name = Utils.GetStr });
            await rep.Create(app);

            app = await rep.Find(_appCode);
            Assert.NotNull(app);
            Assert.Equal(app.Environments[0].Code, _envCode);


            var configRepository = scope.GetInstance<ConfigRepository>();

            var config = new Config("test", new System.Version(1, 0), app.Environments[0].EnvironmentId, null);
            configRepository.CreateConfig(config);

            var configc = await scope.GetInstance<ConfigElementRepository>().Create(await new ObjectConfigReader(config).Parse(Data));
            Assert.NotEqual(0, configc.ConfigElementId);
        }

        [Fact]
        public async void DeleteConfig()
        {
            _appCode = Utils.GetStr;
            _envCode = Utils.GetStr;
            _configCode = Utils.GetStr;
            using var scope = GetScope();
            var rep = scope.GetInstance<ConfigElementRepository>();

            var conf = await rep.GetConfigElement(3);
            Log.WriteLine(conf);
        }

        public static readonly string Json =
@"{
  ""Default"": ""Information"",
  ""Microsoft"": ""Warning"",
  ""Microsoft.Hosting.Lifetime"": ""Information""
}";
        public static readonly string Data = @"{
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
