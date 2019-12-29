using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObjectConfig.Data
{
    public class Application
    {
        [Key]
        public Guid ApplicationId { get; set; }
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        [Required]
        [MaxLength(64)]
        public string Code { get; set; }
        [MaxLength(512)]
        public string Description { get; set; }

        public virtual IList<Environment> Environments { get; set; } = new List<Environment>();

        public virtual IList<UsersApplications> Users { get; set; } = new List<UsersApplications>();
    }
}
