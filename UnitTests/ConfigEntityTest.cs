using ObjectConfig.Data;
using System;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests
{
    public class ConfigEntityTest : BaseTest
    {
        public ConfigEntityTest(ITestOutputHelper output) : base(output)
        {

        }

        [Theory]
        [InlineData("0.1", "0.1.0")]
        [InlineData("0.00.10", "0.00.10")]
        [InlineData("0.00.10.654", "0.0.10")]
        [InlineData("10.10.10","10.10.10")]
        [InlineData("10.10.10.456", "10.10.10")]
        [InlineData("10.10.10.2147483647", "10.10.10")]
        [InlineData("65535.65535.65535.2147483647", "65535.65535.65535")]
        [InlineData("1.1", "1.1.0")]
        [InlineData("10.10", "10.10.0")]
        [InlineData("65535.65535", "65535.65535.0")]
        [InlineData("333.0.0.0", "333.0.0")]
        [InlineData("333.0.2222.0", "333.0.2222")]
        public void TestVersionConverter(string original, string expectedResult) 
        {
            Version originalVersion = new Version(original);
            Version expectedRresultVersion = new Version(expectedResult);

            long longVersion = Config.ConvertVersionToLong(originalVersion);

            Version actualVersion = Config.ConvertLongToVersion(longVersion);

            Log.WriteLine($"Original {originalVersion}: expected: {expectedRresultVersion}; long:{longVersion}; actual: {actualVersion}");
            Assert.Equal(expectedRresultVersion, actualVersion);
        }

        [Theory]
        [InlineData("2147483647.0")]
        [InlineData("65536.65535")]
        [InlineData("65535.65536")]
        [InlineData("65535.65535.2147483647")]
        public void TestInvalidVersionConverter(string original)
        {
            Version originalVersion = new Version(original);
            Assert.Throws<ArgumentException>(()=> Config.ConvertVersionToLong(originalVersion));
        }
    }
}
