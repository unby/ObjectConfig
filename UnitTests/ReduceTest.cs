using Newtonsoft.Json.Linq;
using ObjectConfig;
using ObjectConfig.Data;
using System;
using UnitTests.Data;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests
{
    public class ReduceTest : BaseTest
    {

        protected Config TestConfig = new Config("test", new Version(1, 1), 1, "test");
        protected ConfigElement GetConfigElement(object data)
        {
            return new ObjectConfigReader(TestConfig).Parse(data).Result;
        }
        public ReduceTest(ITestOutputHelper output) : base(output)
        {
        }
        [Fact]
        public void Reduce()
        {
            var fjo = JObject.FromObject(new TestEntity());
            var sjo = JObject.FromObject(new TestEntity());
            Log.WriteLine(fjo);

            Log.WriteLine(sjo);
            Assert.Equal(fjo, sjo);
        }

        [Fact]
        public async void ParseObject()
        {
            var reader = new ObjectConfigReader(TestConfig);
            var origin = JObject.FromObject(new TestEntity());
            ConfigElement configElemnt = await reader.Parse(origin);
            Assert.Equal(TypeNode.Root, configElemnt.Type.Type);

            Log.WriteLine(origin);
            var reducerJobject = new JsonReducer().Parse(configElemnt).Result;

            Log.WriteLine(reducerJobject);
            Assert.Equal(origin.ToString(), reducerJobject.ToString());
        }

        /*
        [Fact]
        public void TestDesiralize()
        {
            var SubSubEntity = new SubSubEntity();
            var EntityName = new JProperty("EntityName", SubSubEntity.EntityName);
            var DoubleField = new JProperty("DoubleField", SubSubEntity.DoubleField);
            var GuidField = new JProperty("GuidField", SubSubEntity.GuidField);
            var FloatField = new JProperty("FloatField", SubSubEntity.FloatField);
            var LongField = new JProperty("LongField", SubSubEntity.LongField);
            JObject SubSubEntityJObject = new JObject(EntityName, GuidField, LongField, FloatField, DoubleField);

            var ThirdEntity = new ThirdEntity();
            JObject ThirdEntityJObject = new JObject(new JProperty("EntityName", ThirdEntity.EntityName), new JProperty("SimpleArray", ThirdEntity.SimpleArray[0], ThirdEntity.SimpleArray[1]));

            var SecondEntity = new SecondEntity();
            var SubArrayEntity = new JObject(new JProperty("SubArrayEntity", "SubArrayEntity"));
            var list = new JProperty("List",
                    new JObject(new JProperty("SubArrayEntity", SubArrayEntity), new JProperty("TimeSpanField", SecondEntity.List[0].TimeSpanField), new JProperty("UriField", SecondEntity.List[0].UriField)),
                    new JObject(new JProperty("SubArrayEntity", SubArrayEntity), new JProperty("TimeSpanField", SecondEntity.List[1].TimeSpanField), new JProperty("UriField", SecondEntity.List[1].UriField)));
            JObject SecondEntityJObject = new JObject(new JProperty("EntityName", SecondEntity.EntityName), new JProperty("BoolField", SecondEntity.BoolField), list);
            var origin = JObject.FromObject(SecondEntity);
            Log.WriteLine(origin);

            Log.WriteLine(SecondEntityJObject);
            Assert.Equal(origin, SecondEntityJObject);
        }
        */


        [Fact]
        public void CompareWithStaticString()
        {
            var fjo = JObject.FromObject(new TestEntity());

            Assert.NotEqual(fjo.ToString(), SpecificPropertySequence);

            var staticObj = JObject.Parse(SpecificPropertySequence);

            Log.WriteLine("staticObj: " + staticObj);
            Log.WriteLine("fjo: " + fjo);


            Assert.NotEqual(fjo, staticObj);

        }

        public const string SpecificPropertySequence = @"{
  ""StringField"": ""stringValue"",
  ""SubEntity"": {
    ""EntityName"": ""SubEntity"",
    ""Date"": ""2020-01-03T00:00:00"",
    ""DateAndTime"": ""2020-01-04T11:55:37.687"",
    ""DateAndTimeOffSet"": ""2020-01-02T11:55:27.045+05:00"",
    ""SubSubEntity"": {
      ""EntityName"": ""SubSubEntity"",
      ""GuidField"": ""0adcd74b-bf1a-4ae4-9b77-6a82ccb80846"",
      ""LongField"": 100000000000,
      ""FloatField"": 10000.133,
      ""DoubleField"": 20000.1325454
    }
  },
  ""ThirdEntity"": {
    ""EntityName"": ""ThirdEntity"",
    ""SimpleArray"": [
      ""SimpleArrayV1"",
      ""SimpleArrayV1""
    ]
  },
  ""SecondEntity"": {
    ""EntityName"": ""SecondEntity"",
    ""BoolField"": true,
    ""List"": [
      {
        ""UriField"": ""http://test.com/tre"",
        ""TimeSpanField"": ""02:30:00""
      },
      {
        ""UriField"": ""https://test2.com/tre"",
        ""TimeSpanField"": ""00:02:30""
      }
    ]
  }
}";
    }
}

