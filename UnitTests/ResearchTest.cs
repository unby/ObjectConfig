using Newtonsoft.Json.Linq;
using ObjectConfig;
using ObjectConfig.Data;
using System;
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
        public void ParseVer()
        {
            Version ver1 = new Version("65636.65536.65536.65536");
            Log.WriteLine(ver1);
            var decimalVersion= Config.ConvertVersionToLong(ver1);
            Log.WriteLine(decimalVersion);
            Config.ConvertLongToVersion(decimalVersion);

            Assert.Equal(ver1, Config.ConvertLongToVersion(decimalVersion));
        }
        [Fact]
        public void ParseVer2()
        {
            Version ver1 = new Version("922299.99999.9999.9999");
            Log.WriteLine(ver1);
            var decimalVersion = Config.ConvertVersionToLong(ver1);
            Log.WriteLine(decimalVersion);
            Config.ConvertLongToVersion(decimalVersion);

            Assert.Equal(ver1, Config.ConvertLongToVersion(decimalVersion));
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
