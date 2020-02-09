using System;
using System.Diagnostics;

namespace ObjectConfig.Data
{
    [DebuggerDisplay("Name = {Name} Type = {Type}")]
    public class TypeElement
    {
        public TypeElement()
        {
            Type = TypeNode.Root;
            Name = "root";
        }

        public TypeElement(TypeNode typeNode, string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"Constructor requires data for {nameof(TypeElement)}'s", nameof(name));
            }

            Type = typeNode != TypeNode.Root ? typeNode : throw new ArgumentException("typeNode dont has 'TypeNode.Root' value's");
            Name = name;
            Description = description;
        }

        public TypeElement(long typeElementId, string name, string? description, TypeNode typeNode)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"Constructor requires data for {nameof(TypeElement)}'s", nameof(name));
            }

            Type = typeNode != TypeNode.Root ? typeNode : throw new ArgumentException("typeNode dont has 'TypeNode.Root' value's");
            TypeElementId = typeElementId;
            Name = name;
            Description = description;
        }

        public long TypeElementId { get; protected set; }

        public string Name { get; protected set; }

        public string? Description { get; protected set; }

        public TypeNode Type { get; protected set; }
    }
}
