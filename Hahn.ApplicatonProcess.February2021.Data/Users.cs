using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hahn.ApplicatonProcess.February2021.Data
{
    public class Users
    {
        public Users()
        {
            Roles = new List<UserRoles>();
        }
        public int Id { get; set; }
        [Required]
        public string EMail { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public bool IsDeleted { get; set; }
        public virtual IList<UserRoles> Roles { get; set; }
    }
}
