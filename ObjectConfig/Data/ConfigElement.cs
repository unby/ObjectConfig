using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace ObjectConfig.Data
{
    [DebuggerDisplay("{Type}; Parrent = {Parrent}")]
    public class ConfigElement
    {
        [Key]
        public Guid ConfigElementId { get; set; }
        public virtual Config Config { get; set; }
        public virtual TypeElement Type { get; set; }
        public virtual List<ValueElement> Value { get; set; } = new List<ValueElement>();
        public virtual List<ConfigElement> Childs { get; set; } = new List<ConfigElement>();
        public virtual ConfigElement Parrent { get; set; }
    }
}
