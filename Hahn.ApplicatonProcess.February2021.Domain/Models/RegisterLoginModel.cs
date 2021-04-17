using System.ComponentModel.DataAnnotations;

namespace Hahn.ApplicatonProcess.February2021.Domain.Models
{
    public class RegisterLoginModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}
