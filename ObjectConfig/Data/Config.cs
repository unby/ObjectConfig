using System;
using System.Collections.Generic;

namespace ObjectConfig.Data
{
    public class Config
    {
        private Config() { }

        public Config(string code, Version versionFrom, int environmentId, string description)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException($"Constructor requires data for {nameof(Config)}'s", nameof(code));
            }

            if (versionFrom is null)
            {
                throw new ArgumentNullException($"Constructor requires data for {nameof(Config)}'s", nameof(versionFrom));
            }

            VersionFrom = ConvertVersionToLong(versionFrom);
            Code = code;
            EnvironmentId = environmentId;
            Description = description;
        }

        public int ConfigId { get; protected set; }

        public string Code { get; protected set; }

        public DateTimeOffset DateFrom { get; protected set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset? DateTo { get; protected set; }

        public long VersionFrom { get; protected set; } = MajorSection;

        public long? VersionTo { get; protected set; }

        public string Description { get; protected set; }

        public int EnvironmentId { get; protected set; }

        public Environment Environment { get; protected set; }

        public List<ConfigElement> ConfigElement { get; protected set; } = new List<ConfigElement>();

        public ConfigElement RootConfigElement { get; set; }

        public Version GetVersionFrom { get { return ConvertLongToVersion(VersionFrom); } }
        public Version GetVersionTo { get { return ConvertLongToVersion(VersionTo); } }

        public void SetVersionFrom(Version version)
        {
            VersionFrom = ConvertVersionToLong(version ?? Default);
        }

        public void SetVersionTo(Version version)
        {
            if (version != null)
                VersionFrom = ConvertVersionToLong(version);
        }

        static Version Default = new Version(1, 0, 0);
        const long MinorSection = 100000;
        const long MajorSection = 100000 * MinorSection;

        public static Version ConvertLongToVersion(long? version)
        {
            if (version.HasValue)
            {
                int major, minor, build;

                major = (int)(version / MajorSection);
                minor = (int)((version - major * MajorSection) / MinorSection);
                build = (int)((version - major * MajorSection - minor * MinorSection));

                return new Version(major, minor, build);
            }

            return null;
        }

        /// <summary>
        /// Read Major, Minor and Build section, Revison skiped
        /// </summary>
        /// <param name="version">max version '65536.65536.65536' </param>
        /// <returns>Value between 1 and 655 356 553 565 535</returns>
        public static long ConvertVersionToLong(Version version)
        {
            const int sectionSize = ushort.MaxValue;
            long res = MajorSection;

            if (version.Major <= sectionSize)
                res = version.Major * MajorSection;
            else
                throw new ArgumentException($"Major values ({version.Major}) must not be greater than {sectionSize}");

            if (version.Minor <= sectionSize)
                res = res + MinorSection * version.Minor;
            else
                throw new ArgumentException($"Minor values ({version.Minor}) must not be greater than {sectionSize}");

            if (version.Build <= sectionSize)
                res = res + (version.Build > -1 ? version.Build : 0);
            else
                throw new ArgumentException($"Build values ({version.Build}) must not be greater than {sectionSize}");

            return res;
        }
    }
}
