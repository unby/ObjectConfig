using System;
using System.Diagnostics;

namespace ObjectConfig.Data
{
    [DebuggerDisplay("Value = {Value} Type = {Type}")]
    public class ValueElement
    {
        private ValueElement() 
        {
            DateFrom = DateTimeOffset.UtcNow;
        }

        public ValueElement(string value, TypeElement type)
        {
            DateFrom = DateTimeOffset.UtcNow;
            Value = value;
            Type = type;
        }

        public long ValueElementId { get; protected set; }

        public string Value { get; protected set; }

        public string Comment { get; protected set; }

        public DateTimeOffset DateFrom { get; protected set; }

        public DateTimeOffset? DateTo { get; protected set; }

        public virtual TypeElement Type { get; protected set; }

        public virtual User ChangeOwner { get; protected set; }
    }
}
