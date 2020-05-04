using System;

namespace ObjectConfig.Data
{
    public interface IPeriod
    {
        DateTimeOffset DateFrom { get; }
        DateTimeOffset? DateTo { get; }

        void Close(DateTimeOffset closeDate);
    }
}
