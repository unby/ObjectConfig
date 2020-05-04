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

        public ValueElement(string? value, ConfigElement element,  DateTimeOffset dateFrom)
        {
            ConfigElement = element ?? throw new ArgumentNullException(nameof(element));
            DateFrom = dateFrom;
            Value = value;
        }

        public long ValueElementId { get; protected set; }
        public long ConfigElementId { get; protected set; }

        public ConfigElement ConfigElement { get; protected set; }
        public string? Value { get; protected set; }

        public string? Comment { get; protected set; }

        public DateTimeOffset DateFrom { get; protected set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset? DateTo { get; protected set; }

        public int? ChangeOwnerUserId { get; protected set; }

        public virtual User ChangeOwner { get; protected set; }

        public object? GetObjectValue()
        {
            switch (ConfigElement.TypeElement.Type)
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

        public void Close(DateTimeOffset closeDate)
        {
            DateTo = closeDate;
        }
    }
}
