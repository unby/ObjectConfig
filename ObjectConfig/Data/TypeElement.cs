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

        public int TypeElementId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public TypeNode Type { get; set; }
    }
}
