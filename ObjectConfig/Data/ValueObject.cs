using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace ObjectConfig.Data
{
    [DebuggerDisplay("Value = {Value} Type = {Type}")]
    public class ValueElement
    {
        public ValueElement() 
        {
            DateFrom = DateTimeOffset.UtcNow;
        }

        public ValueElement(string value, TypeElement type)
        {
            DateFrom = DateTimeOffset.UtcNow;
            Value = value;
            Type = type;
        }

        [Key]
        public Guid ValueElementId { get; set; }

        [Required]
        [MaxLength(int.MaxValue)]
        public string Value { get; set; }
        public DateTimeOffset DateFrom { get; set; }

        public DateTimeOffset? DateTo { get; set; }

        public virtual TypeElement Type { get; set; }

        public virtual User ChangeOwner { get; set; }
    }
}
