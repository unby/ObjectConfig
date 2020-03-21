namespace ObjectConfig.Data
{
    public class ConfigCache
    {
        private ConfigCache()
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

        public int ConfigId { get; protected set; }

        public int ConfigCacheId { get; protected set; }
        public Config Config { get; protected set; }
        public string ConfigValue { get; protected set; }
    }
}
