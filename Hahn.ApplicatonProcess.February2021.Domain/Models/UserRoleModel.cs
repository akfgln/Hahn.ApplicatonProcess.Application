namespace Hahn.ApplicatonProcess.February2021.Domain.Models
{
    public class UserRoleModel
    {
        public int UserRoleId { get; set; }
        public bool IsActive { get; set; }
        public int? RoleId { get; set; }
        public int UserId { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
    }
}
