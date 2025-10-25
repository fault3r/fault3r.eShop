
using System;

namespace AccountService.Domain.Entities
{
    public class Account : BaseEntity
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }

        public Account(string email, string password, Role role)
        {
            Id = Guid.NewGuid();
            Email = email;
            Password = password;
            Name = Email.Split('@')[0];
            Role = role;
        }
    }
}