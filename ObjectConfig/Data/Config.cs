﻿using System;
using System.Collections.Generic;

namespace ObjectConfig.Data
{
    public class Config : IPeriod
    {
        private Config()
        {
        }

        public Config(string code, Environment environment, long versionFrom = _majorSection, long? versionTo = null)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException($"Constructor requires data for {nameof(Config)}'s", nameof(code));
            }

            if (environment is null)
            {
                throw new ArgumentNullException($"Constructor requires data for {nameof(Config)}'s", nameof(environment));
            }

            if (versionTo.HasValue && versionTo > 0)
            {
                VersionTo = versionTo;
            }

            VersionFrom = versionFrom;
            Code = code;
            Environment = environment;
        }

        public Config(string code, Version versionFrom, int environmentId, string? description)
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

        public void Close(DateTimeOffset closeDate)
        {
            DateTo = closeDate;
        }

        public long VersionFrom { get; protected set; } = _majorSection;

        public long? VersionTo { get; protected set; }

        public string? Description { get; protected set; }

        public int EnvironmentId { get; protected set; }

        public Environment Environment { get; protected set; }

        public List<ConfigElement> ConfigElement { get; protected set; } = new List<ConfigElement>();

        public ConfigElement RootConfigElement { get; set; }

        public Version GetVersionFrom => ConvertLongToVersion(VersionFrom);
        public Version? GetVersionTo => ConvertLongToVersion(VersionTo);

        public void SetVersionFrom(Version version)
        {
            VersionFrom = ConvertVersionToLong(version ?? _default);
        }

        public void SetVersionTo(Version version)
        {
            if (version != null)
            {
                VersionTo = ConvertVersionToLong(version);
            }
        }

        private const long _minorSection = 100000;
        public const long _majorSection = 100000 * _minorSection;
        public static readonly Version _default = ConvertLongToVersion(_majorSection);

        public static Version? ConvertLongToVersion(long? version)
        {
            if (version.HasValue)
            {
                return ConvertLongToVersion(version.Value);
            }

            return null;
        }

        public static Version ConvertLongToVersion(long version)
        {
            int major, minor, build;

            major = (int)(version / _majorSection);
            minor = (int)((version - (major * _majorSection)) / _minorSection);
            build = (int)(version - (major * _majorSection) - (minor * _minorSection));

            return new Version(major, minor, build);
        }

        /// <summary>
        /// Read Major, Minor and Build section, Revison skiped
        /// </summary>
        /// <param name="version">max version '65536.65536.65536' </param>
        /// <returns>Value between 1 and 655 356 553 565 535</returns>
        public static long ConvertVersionToLong(Version version)
        {
            const int sectionSize = ushort.MaxValue;
            long res;
            if (version.Major <= sectionSize)
            {
                res = version.Major * _majorSection;
            }
            else
            {
                throw new ArgumentException($"Major values ({version.Major}) must not be greater than {sectionSize}");
            }

            if (version.Minor <= sectionSize)
            {
                res += _minorSection * version.Minor;
            }
            else
            {
                throw new ArgumentException($"Minor values ({version.Minor}) must not be greater than {sectionSize}");
            }

            if (version.Build <= sectionSize)
            {
                res += version.Build > -1 ? version.Build : 0;
            }
            else
            {
                throw new ArgumentException($"Build values ({version.Build}) must not be greater than {sectionSize}");
            }

            return res;
        }
    }
}
