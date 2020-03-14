using ObjectConfig.Data;
using System;

namespace ObjectConfig.Features.Configs
{
    public class ConfigDto
    {
#nullable disable
        private ConfigDto()
        {
        }
#nullable enable

        public ConfigDto(Config config)
        {
            Code = config.Code;
            DateFrom = config.DateFrom;
            DateTo = config.DateTo;
            VersionFrom = config.GetVersionFrom.ToString();
            VersionTo = config.GetVersionTo?.ToString();
            Description = config.Description;
            EnvironmentId = config.EnvironmentId;
            ConfigId = config.ConfigId;
        }

        public int ConfigId { get; protected set; }

        public string Code { get; protected set; }

        public DateTimeOffset DateFrom { get; protected set; }

        public DateTimeOffset? DateTo { get; protected set; }

        public string VersionFrom { get; protected set; }

        public string? VersionTo { get; protected set; }

        public string? Description { get; protected set; }

        public int EnvironmentId { get; protected set; }
    }
}
