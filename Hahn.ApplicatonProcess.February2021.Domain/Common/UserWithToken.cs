using Hahn.ApplicatonProcess.February2021.Data;
using System;

namespace Hahn.ApplicatonProcess.February2021.Domain.Common
{
    public class UserWithToken
    {
        public string Token { get; set; }
        public Users User { get; set; }
        public DateTime Expiration { get; set; }
    }
}
