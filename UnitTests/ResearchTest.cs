using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests
{
    public class ResearchTest : BaseTest
    {
        public ResearchTest(ITestOutputHelper output)
            : base(output)
        {
        }

        [Fact]
        public void Decode()
        {
            JObject stuff = JObject.Parse(Data);
            Assert.Equal(true, stuff["Logging"]["GlobalLevel"]);
            JToken ll = stuff["Logging"]["LogLevel"];
            Log.WriteLine(ll);
            Log.WriteLine(LogLevel);
            Assert.Equal(LogLevel, ll);
        }

        public static readonly string LogLevel =
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