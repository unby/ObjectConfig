using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace ObjectConfig.Data
{
    public enum TypeNode 
    {
        None = 0, 
        
        Complex = 1, 
        
        Array = 2,

        Integer = 6,

        Float = 7,

        String = 8,

        Boolean = 9,

        Null = 10,
      
        Date = 12,    
     
        Guid = 15,

        Uri = 16,

        TimeSpan = 17,

        Root = 30
    }
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
                throw new System.ArgumentException("message", nameof(name));
            }
            Name = name;
            Type = typeNode != TypeNode.Root ? typeNode : throw new ArgumentException("typeNode dont has 'TypeNode.Root' value's");
        }

        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)
        public Guid TypeElementId { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }
        [MaxLength(512)]
        public string Description { get; set; }
        public TypeNode Type { get; set; }
    }
}
