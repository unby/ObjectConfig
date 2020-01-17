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

        public ValueElement(string value, TypeElement type) : this()
        {
            Value = value;
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        public long ValueElementId { get; protected set; }

        public string Value { get; protected set; }

        public string Comment { get; protected set; }

        public DateTimeOffset DateFrom { get; protected set; }

        public DateTimeOffset? DateTo { get; protected set; }

        public virtual TypeElement Type { get; protected set; }

        public int? ChangeOwnerUserId { get; protected set; }

        public virtual User ChangeOwner { get; protected set; }

        public object GetObjectValue()
        {
            switch (Type.Type)
            {
                case TypeNode.Root:
                case TypeNode.Complex:
                case TypeNode.Array:
                    throw new Exception();
                case TypeNode.Integer:
                    return int.Parse(Value);
                case TypeNode.Float:
                    return float.Parse(Value);
                case TypeNode.String:
                    return Value;
                case TypeNode.Boolean:
                    return bool.Parse(Value);
                case TypeNode.Date:
                    return DateTimeOffset.Parse(Value);
                case TypeNode.Guid:
                    return Guid.Parse(Value);
                case TypeNode.Uri:
                    return new Uri(Value);
                case TypeNode.TimeSpan:
                    return TimeSpan.Parse(Value);

                default:
                    return null;
            }
        }
    }
}
