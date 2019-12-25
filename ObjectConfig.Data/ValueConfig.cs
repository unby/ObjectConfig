using System.ComponentModel.DataAnnotations;

namespace ObjectConfig.Data
{
    public class ValueConfig
    {
        [Key]
        public int ValueConfigId { get; set; }

        public int ParrentValueConfigId { get; set; }
        public virtual Config Config { get; set; }

        public virtual ValueType Type { get; set; }
        public virtual ValueConfig ParrentProperty { get; set; }
    }
}
