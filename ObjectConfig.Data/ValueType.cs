using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObjectConfig.Data
{
    public class ValueType
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)
        public int ValueTypeId { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }
        [MaxLength(512)]
        public string Description { get; set; }
        public int Type { get; set; }

        public virtual IList<ValueObject> ValueObjects { get; set; }
    }
}
