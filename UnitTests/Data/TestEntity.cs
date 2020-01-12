using System;

namespace UnitTests.Data
{
    public class SubArrayEntity
    {
        public string EntityName { get; set; } = nameof(SubArrayEntity);
    }

    public class ArrayEntity
    {
        public SubArrayEntity SubArrayEntity { get; set; } = new SubArrayEntity();
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

}
