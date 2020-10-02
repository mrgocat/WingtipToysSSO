using System;
using System.Collections.Generic;

namespace WingtipSSO.POCOS
{
    public class UserPoco
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Telephone { get; set; }
        public bool ForceChangePassword { get; set; }
        public string Nationality { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool IsUser { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActivated { get; set; }
        public bool IsLocked { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
    public enum Role{
        Admin, Moderator, 
    }
}
