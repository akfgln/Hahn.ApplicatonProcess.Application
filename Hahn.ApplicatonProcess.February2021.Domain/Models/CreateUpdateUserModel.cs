﻿using System.ComponentModel.DataAnnotations;

namespace Hahn.ApplicatonProcess.February2021.Domain.Models
{
    public class CreateUpdateUserModel
    {
        public CreateUpdateUserModel()
        {
            Roles = new string[0];
        }

        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string[] Roles { get; set; }
    }
}
