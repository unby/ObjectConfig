using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using ObjectConfig.Model;

namespace UnitTests
{
    public class ApplictionRepositoryTest : BaseTest
    {
        public ApplictionRepositoryTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void CreateApp() 
        {
            using (var scope = GetScope())
            {
                var rep = scope.GetInstance<ApplicationRepository>();

                Assert.NotNull(rep);
            }
          
        }
    }
}
/*,
""Array"": [
""123"",
""sto""
]
*/
