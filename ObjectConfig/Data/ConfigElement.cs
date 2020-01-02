using System.Collections.Generic;
using System.Diagnostics;

namespace ObjectConfig.Data
{
    [DebuggerDisplay("{Type}; Parrent = {Parrent}")]
    public class ConfigElement
    {
        private ConfigElement() { }

        public ConfigElement(TypeElement typeElement, ConfigElement parrent, Config config)
        {
            Type = typeElement;
            Parrent = parrent;
            Config = config;
        }

        public ConfigElement(TypeElement typeElement, Config config)
        {
            Type = typeElement;
            Config = config;
        }

        public long ConfigElementId { get; set; }

        public int ConfigId { get; set; }

        public Config Config { get; set; }

        public TypeElement Type { get; set; }
       // public string Path { get; set; }

        public long? ParrentConfigElementId { get; set; }

        public ConfigElement Parrent { get; set; }

        public List<ValueElement> Value { get; set; } = new List<ValueElement>();

        public List<ConfigElement> Childs { get; set; } = new List<ConfigElement>();
    }
}
