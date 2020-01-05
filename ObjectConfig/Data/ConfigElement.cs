using System.Collections.Generic;
using System.Diagnostics;

namespace ObjectConfig.Data
{
    [DebuggerDisplay("{Type}; Parrent = {Parrent}")]
    public class ConfigElement
    {
        private ConfigElement() { }

        public ConfigElement(TypeElement typeElement, ConfigElement parrent, Config config, string path ) 
        {
            Type = typeElement;
            Parrent = parrent;
            Config = config;
            Path = path;
        }

        public long ConfigElementId { get; set; }

        public int ConfigId { get; set; }

        public string Path { get; set; }

        public Config Config { get; set; }

        public TypeElement Type { get; set; }

        public long? ParrentConfigElementId { get; set; }

        public ConfigElement Parrent { get; set; }

        public List<ValueElement> Value { get; set; } = new List<ValueElement>();

        public List<ConfigElement> Childs { get; set; } = new List<ConfigElement>();
    }
}
