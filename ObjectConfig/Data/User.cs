using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObjectConfig.Data
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        [Required]
        public string ExternalId { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public bool IsGlobalAdmin { get; set; }

        public virtual List<UsersApplications> Applications { get; set; } = new List<UsersApplications>();

        public virtual List<UsersEnvironments> Environments { get; set; } = new List<UsersEnvironments>();
    }
}
