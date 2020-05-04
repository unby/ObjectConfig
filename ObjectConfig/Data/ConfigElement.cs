using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ObjectConfig.Data
{
    [DebuggerDisplay("{TypeElement}; Parrent = {Parrent}")]
    public class ConfigElement
    {
        private ConfigElement()
        {
        }

        public ConfigElement(TypeElement typeElementElement, ConfigElement? parrent, Config config, string? path)
        {
            TypeElement = typeElementElement;
            Parrent = parrent;
            Config = config;
            Path = path ?? ".";
        }

        public long ConfigElementId { get; set; }

        public int ConfigId { get; set; }

        public string Path { get; set; }

        public Config Config { get; set; }

        public long TypeElementId { get; protected set; }
        public TypeElement TypeElement { get; set; }

        public long? ParrentConfigElementId { get; set; }

        public ConfigElement? Parrent { get; set; }

        public List<ValueElement> Value { get; set; } = new List<ValueElement>();

        public List<ConfigElement> Childs { get; set; } = new List<ConfigElement>();

        public void CopyTo(Config config)
        {
            ConfigId = config.ConfigId;
            Config = config;
        }
    }
}
