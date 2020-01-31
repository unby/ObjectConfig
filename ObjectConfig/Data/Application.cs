using System;
using System.Collections.Generic;

namespace ObjectConfig.Data
{
    public class Application : IUsers<UsersApplications, UsersApplications.Role>
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        private Application()
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {
        }

        public Application(int applicationId, string name, string code, string? description) : this(applicationId)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new System.ArgumentException("message", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                throw new System.ArgumentException("message", nameof(code));
            }

            Name = name;
            Code = code;
            Description = description;
        }

        public Application(string name, string code, string? description)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new System.ArgumentException("message", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                throw new System.ArgumentException("message", nameof(code));
            }

            Name = name;
            Code = code;
            Description = description;
        }

        public Application(int applicationId)
        {
            ApplicationId = applicationId;
        }

        public void Rename(string name)
        {
            Name = name;
        }

        public int ApplicationId { get; protected set; }

        public void NewDefinition(string? description)
        {
            Description = description;
        }

        public string Name { get; protected set; }

        public string Code { get; protected set; }

        public string? Description { get; protected set; }

        public virtual IList<Environment> Environments { get; set; } = new List<Environment>();

        public virtual IList<UsersApplications> Users { get; set; } = new List<UsersApplications>();
    }
}
