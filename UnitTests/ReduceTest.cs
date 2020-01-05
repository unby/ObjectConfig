using Newtonsoft.Json.Linq;
using ObjectConfig;
using ObjectConfig.Data;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests
{
  
   
    public class ArrayEntity
    {
        public Uri UriField { get; set; }
        public TimeSpan TimeSpanField { get; set; }
    }

    public class ThirdEntity
    {
        public string EntityName { get; set; } = nameof(ThirdEntity);

        public string[] SimpleArray { get; set; } = new string[] { "SimpleArrayItem1", "SimpleArrayItem2" };
    }

    public class SecondEntity
    {
        public SecondEntity()
        {
            List = new ArrayEntity[]
            { 
                new ArrayEntity() { TimeSpanField = TimeSpan.FromMinutes(150), UriField = new Uri("http://test.com/tre") },
                new ArrayEntity() { TimeSpanField = TimeSpan.FromSeconds(150), UriField = new Uri("https://test2.com/tre") }
            };
        }
        public string EntityName { get; set; } = nameof(SecondEntity);
        public bool BoolField { get; set; } = true;

        public ArrayEntity[] List { get; set; }
    }

    public class SubEntity
    {
        public string EntityName { get; set; } = nameof(SubEntity);
        public DateTime Date { get; set; } = new DateTime(2020, 1, 3);

        public DateTime DateAndTime { get; set; } = new DateTime(2020, 1, 4, 11, 55, 37, 687);

        public DateTimeOffset DateAndTimeOffSet { get; set; } = new DateTimeOffset(2020, 1, 2, 11, 55, 27, 45, TimeSpan.FromHours(5));

        public SubSubEntity SubSubEntity { get; set; } = new SubSubEntity();
    }
    public class TestEntity
    {
        public string StringField { get; set; } = "stringValue";

        public SubEntity SubEntity { get; set; } = new SubEntity();

        public SecondEntity SecondEntity { get; set; } = new SecondEntity();

        public ThirdEntity ThirdEntity { get; set; } = new ThirdEntity();

    }

    public class SubSubEntity
    {
        public static string GuidValue = "0ADCD74B-BF1A-4AE4-9B77-6A82CCB80846";
        public string EntityName { get; set; } = nameof(SubSubEntity);

        public Guid GuidField { get; set; } = Guid.Parse(GuidValue);

        public long LongField { get; set; } = 100000000000;

        public float FloatField { get; set; } = 10000.1325454F;

        public double DoubleField { get; set; } = 20000.1325454;
    }

    public class ReduceTest : BaseTest
    {

        protected ObjectConfig.Data.Config TestConfig = new ObjectConfig.Data.Config("test", new Version(1, 1), 1, "test");
        protected ObjectConfig.Data.ConfigElement GetConfigElement(object data)
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
            Assert.Equal(ObjectConfig.Data.TypeNode.Root, configElemnt.Type.Type);
            Assert.NotNull(reader.AllNodes.FirstOrDefault(f => 
                        f.Type.Name == "GuidField" &&
                        f.Value.Any(a => a.Value.Equals(SubSubEntity.GuidValue,StringComparison.OrdinalIgnoreCase))));
            Log.WriteLine(origin);
            var reducerJobject = new JsonReducer().Parse(configElemnt).Result;

            Log.WriteLine(reducerJobject);
            Assert.Equal(origin, reducerJobject);

        }

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

            var list = new JProperty("List", 
                    new JObject(new JProperty("TimeSpanField", SecondEntity.List[0].TimeSpanField), new JProperty("UriField", SecondEntity.List[0].UriField)),
                    new JObject(new JProperty("TimeSpanField", SecondEntity.List[1].TimeSpanField), new JProperty("UriField", SecondEntity.List[1].UriField)));
            JObject SecondEntityJObject = new JObject(new JProperty("EntityName", SecondEntity.EntityName), new JProperty("BoolField", SecondEntity.BoolField), list);
            var origin = JObject.FromObject(SecondEntity);
            Log.WriteLine(origin);
            
            
         //  
         //  
         // 
            Log.WriteLine(SecondEntityJObject);
         //
            Assert.Equal(origin, SecondEntityJObject);
        }


        [Fact]
        public void CompareWithStaticString()
        {
            var fjo = JObject.FromObject(new TestEntity());

            Assert.NotEqual(fjo.ToString(), SpecificPropertySequence);

            var staticObj = JObject.Parse(SpecificPropertySequence);

            Log.WriteLine("staticObj: "+staticObj);
            Log.WriteLine("fjo: " + fjo);

            
            Assert.NotEqual(fjo, staticObj);

        }

        const string SpecificPropertySequence = @"{
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

