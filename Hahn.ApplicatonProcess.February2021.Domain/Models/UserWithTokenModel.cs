using System;

namespace Hahn.ApplicatonProcess.February2021.Domain.Models
{
    public class UserWithTokenModel
    {
        public string Token { get; set; }
        public UserModel User { get; set; }
        public DateTime Expiration { get; set; }
    }
}
