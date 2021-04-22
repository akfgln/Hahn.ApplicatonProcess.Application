using Hahn.ApplicatonProcess.February2021.Data;
using Hahn.ApplicatonProcess.February2021.Domain.Common;
using Hahn.ApplicatonProcess.February2021.Domain.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Hahn.ApplicatonProcess.February2021.Web.Security
{
    public class PermissionContext : IPermissionContext
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IUnitOfWork uow;
        private Users user;

        public PermissionContext(IHttpContextAccessor contextAccessor, IUnitOfWork uow)
        {
            this.contextAccessor = contextAccessor;
            this.uow = uow;
        }

        public Users User
        {
            get
            {
                if (user != null) return user;

                if (!contextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    throw new UnauthorizedAccessException();
                }

                var email = contextAccessor.HttpContext.User.Identity.Name;
                user = uow.Query<Users>()
                    .Where(x => x.EMail == email)
                    .Include(x => x.Roles)
                    .ThenInclude(x => x.Role)
                    .FirstOrDefault();

                if (user == null)
                {
                    throw new UnauthorizedAccessException("User is not found");
                }

                return user;
            }
        }

        public bool IsAdministrator
        {
            get { return User.Roles.Any(x => x.Role.DefaultRoleName == SystemRoles.Administrator); }
        }
    }
}
