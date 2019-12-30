using Newtonsoft.Json.Linq;
using ObjectConfig;
using ObjectConfig.Data;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests
{
    public class ResearchTest: BaseTest
    {
        public ResearchTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ChekUser()
        {
            using (var context = GetObjectConfigContext())
            {

                var admin = context.Admin();
                Assert.Equal(Constants.AdminId, admin.UserId);
                Assert.Equal("GlobalAdmin", admin.DisplayName);
            }
        }

        [Fact]
        public void Decode() {
            JObject stuff = JObject.Parse(Data);
            Assert.Equal(true, stuff["Logging"]["GlobalLevel"]);
            var ll = stuff["Logging"]["LogLevel"];
            Log.WriteLine(ll);
            Log.WriteLine(LogLevel);
            Assert.Equal(LogLevel,ll);
        }

        [Fact]
        public void Test1()
        {
            using (var context = GetObjectConfigContext())
            {
                var admin = context.Admin();

                context.UsersApplications.Add(new UsersApplications() { User = admin, AccessRole = UsersApplications.Role.Administrator, Application = new Application() { Code = "test", Name = "test title" } });

                context.SaveChanges();
                Assert.True(context.UsersApplications.Any());
                Assert.True(context.Applications.Any());
                var app = context.Applications.First();
                Log.WriteLine(app.ApplicationId);
                Assert.NotNull(app);
            }
        }
      

        [Fact]
        public void Parse()
        {
            var d = new ObjectConfigReader(new Config()).Parse(Data).Result;
            Log.WriteLine(d.Type.Name);
            Assert.Equal("root", d.Type.Name);
        }

        static string LogLevel = 
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
