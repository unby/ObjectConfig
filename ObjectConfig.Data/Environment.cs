using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObjectConfig.Data
{
    public class Environment
    {
        [Key]
        public int EnvironmentId { get; set; }
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        [Required]
        [MaxLength(64)]
        public string Code { get; set; }
        [MaxLength(512)]
        public string Description { get; set; }

        public virtual IList<Config> Configs { get; set; } = new List<Config>();

        public virtual IList<UsersEnvironments> Users { get; set; } = new List<UsersEnvironments>();

        public virtual Application Application { get; set; }
    }
}
