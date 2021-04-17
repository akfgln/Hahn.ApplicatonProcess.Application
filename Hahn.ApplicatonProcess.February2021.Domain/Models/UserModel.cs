
namespace Hahn.ApplicatonProcess.February2021.Domain.Models
{
    public class UserModel
    {
        public UserModel()
        {
            Roles = new string[0];
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string[] Roles { get; set; }
    }
}
