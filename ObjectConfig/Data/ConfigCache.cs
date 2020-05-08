using System;

namespace ObjectConfig.Data
{
    public class ConfigCache
    {
        public ConfigCache()
        {
        }

        public ConfigCache(Config config, string configValue)
        {
            Config = config ??
                     throw new System.ArgumentException(
                         $"Constructor requires data for {nameof(ConfigCache)}'s", nameof(config));
            ConfigValue = configValue ??
                          throw new System.ArgumentException(
                              $"Constructor requires data for {nameof(ConfigCache)}'s", nameof(configValue));
        }

        public ConfigCache(int configId, string configValue, string outType)
        {
            OutType = outType;
            ConfigId = configId;
            ConfigValue = configValue;
        }

        public int ConfigId { get; protected set; }

        public int ConfigCacheId { get; protected set; }
        public Config Config { get; protected set; }

        public string OutType { get; protected set; } = "json";
        public string ConfigValue { get; protected set; }

        public void UpdateValue(string data)
        {
            ConfigValue = data ?? throw new ArgumentNullException(nameof(data));
        }
    }
}
