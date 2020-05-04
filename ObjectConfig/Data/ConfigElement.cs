using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ObjectConfig.Data
{
    [DebuggerDisplay("{Type}; Parrent = {Parrent}")]
    public class ConfigElement
    {
        private ConfigElement()
        {
        }

        public ConfigElement(TypeElement typeElement, ConfigElement? parrent, Config config, string? path)
        {
            Parrent = parrent;
            Config = config;
            Path = path ?? ".";
        }

        public long ConfigElementId { get; set; }

        public int ConfigId { get; set; }

        public string Path { get; set; }

        public Config Config { get; set; }

        public TypeElement Type
        {
            get
            {
                if (Value.Any())
                    return Value[0].Type;
                else if (Path == ".")
                    return TypeElement.Root;
                else if(Childs.Any()&&!Value.Any())
                    return TypeElement.Complex;
                else
                    return TypeElement.None;
            }
        }

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
