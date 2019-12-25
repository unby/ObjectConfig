using System;
using System.ComponentModel.DataAnnotations;

namespace ObjectConfig.Data
{

    public class ValueObject
    {
        [Key]
        public int ValueTypeId { get; set; }

        [Required]
        [MaxLength(int.MaxValue)]
        public string Value { get; set; }
        public DateTimeOffset DateFrom { get; set; }

        public DateTimeOffset? DateTo { get; set; }

        public virtual ValueType Type { get; set; }

        public virtual User ChangeOwner { get; set; }
    }
}
