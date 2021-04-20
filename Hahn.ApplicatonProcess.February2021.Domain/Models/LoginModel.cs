using System.ComponentModel.DataAnnotations;

namespace Hahn.ApplicatonProcess.February2021.Domain.Models
{
    public class LoginModel
    {
        /// <summary>
        /// Email
        /// </summary>
        /// <example>admin@hahn.com</example>
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
