using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hahn.ApplicatonProcess.February2021.Data
{
    public class Roles
    {
        public Roles()
        {
            UserRoles = new List<UserRoles>();
        }
        public int RoleId { get; set; }
        [Required]
        public string DefaultRoleName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public virtual IList<UserRoles> UserRoles { get; set; }
    }
}
