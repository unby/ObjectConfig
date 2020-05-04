using System;
using System.Diagnostics;

namespace ObjectConfig.Data
{
    [DebuggerDisplay("Name = {Name} TypeNode = {TypeNode}")]
    public class TypeElement
    {
        public TypeElement()
        {
            TypeNode = TypeNode.Root;
            Name = "root";
        }

        public TypeElement(TypeNode typeNode, string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"Constructor requires data for {nameof(TypeElement)}'s", nameof(name));
            }

            TypeNode = typeNode != TypeNode.Root ? typeNode : throw new ArgumentException("typeNode dont has 'TypeNode.Root' value's");
            Name = name;
            Description = description;
        }

        public long TypeElementId { get; protected set; }

        public string Name { get; protected set; }

        public string? Description { get; protected set; }

        public TypeNode TypeNode { get; protected set; }
    }
}
