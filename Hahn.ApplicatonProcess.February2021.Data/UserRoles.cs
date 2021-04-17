namespace Hahn.ApplicatonProcess.February2021.Data
{
    public class UserRoles
    {
        public int UserRoleId { get; set; }
        public bool IsActive { get; set; }
        public int? RoleId { get; set; }
        public int UserId { get; set; }
        public virtual Users User { get; set; }
        public virtual Roles Role { get; set; }
    }
}
