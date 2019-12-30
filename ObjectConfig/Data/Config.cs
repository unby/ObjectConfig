using System;

namespace ObjectConfig.Data
{
    public class Config
    {
        public int ConfigId { get; set; }

        public string Code { get; set; }

        public DateTimeOffset DateFrom { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset? DateTo { get; set; }

        public string VersionFrom { get; set; } = "1.0";

        public string VersionTo { get; set; }

        public string Description { get; set; }

        public int EnvironmentId { get; set; }

        public Environment Environment { get; set; }

        public int ConfigElementId { get; set; }

        public ConfigElement ConfigElement { get; set; }
    }
}
