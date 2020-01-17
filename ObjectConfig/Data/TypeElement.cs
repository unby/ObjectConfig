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

        public TypeElement(TypeNode typeNode, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("message", nameof(name));
            }
            Name = name;
            Type = typeNode != TypeNode.Root ? typeNode : throw new ArgumentException("typeNode dont has 'TypeNode.Root' value's");
        }

        public long TypeElementId { get; protected set; }

        public string Name { get; protected set; }

        public string Description { get; protected set; }

        public TypeNode Type { get; protected set; }
    }
}
