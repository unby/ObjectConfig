using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObjectConfig.Data
{
    public class Config
    {
        [Key]
        public Guid ConfigId { get; set; }

        [Required]
        [MaxLength(128)]
        public string Code { get; set; }

        public DateTimeOffset DateFrom { get; set; }

        public DateTimeOffset? DateTo { get; set; }

        [Required]
        [MaxLength(23)]
        public string VersionFrom { get; set; }

        [MaxLength(23)]
        public string VersionTo { get; set; }

        [MaxLength(512)]
        public string Description { get; set; }

        public virtual Environment Environment { get; set; }

        public Guid EnvironmentId { get; set; }

        public virtual IList<ConfigElement> Configs { get; set; }
    }
}
