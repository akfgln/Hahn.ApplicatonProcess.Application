using Hahn.ApplicatonProcess.February2021.Data;
using Hahn.ApplicatonProcess.February2021.Data.Helpers;
using Hahn.ApplicatonProcess.February2021.Domain.Common;
using Hahn.ApplicatonProcess.February2021.Domain.Exceptions;
using Hahn.ApplicatonProcess.February2021.Domain.Models;
using Hahn.ApplicatonProcess.February2021.Domain.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Domain
{

    public class UserRepository : IUserRepository
    {
        private readonly IUnitOfWork uow;
        private readonly ISecurityContext securityContext;

        public UserRepository(IUnitOfWork uow, ISecurityContext securityContext)
        {
            this.uow = uow;
            this.securityContext = securityContext;
        }

        public IQueryable<Users> Get()
        {
            var query = GetQuery();
            return query;
        }

        private IQueryable<Users> GetQuery()
        {
            return uow.Query<Users>()
                .Where(x => !x.IsDeleted)
                .Include(x => x.Roles)
                    .ThenInclude(x => x.Role);
        }

        public Users Get(int id)
        {
            var user = GetQuery().FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                throw new NotFoundException("User is not found!");
            }

            return user;
        }

        public async Task<Users> Create(CreateUpdateUserModel model)
        {
            var email = model.Email.Trim();
            if (GetQuery().Any(u => u.EMail == email))
            {
                throw new BadRequestException("The email is already in use");
            }

            var user = new Users
            {
                EMail = model.Email.Trim(),
                Password = model.Password.Trim().WithBCrypt(),
                FirstName = model.FirstName.Trim(),
                LastName = model.LastName.Trim(),
            };
            AddUserRoles(user, model.Roles);
            uow.Add(user);
            await uow.CommitAsync();
            return user;
        }

        private void AddUserRoles(Users user, string[] roleNames)
        {
            if (roleNames.Any() && !securityContext.IsAdministrator)
                throw new ForbiddenException("Unauthorized operation!");

            user.Roles.Clear();

            foreach (var roleName in roleNames)
            {
                var role = uow.Query<Roles>().FirstOrDefault(x => x.DefaultRoleName == roleName);
                if (role == null)
                {
                    throw new NotFoundException($"Role - {roleName} is not found");
                }
                user.Roles.Add(new UserRoles { User = user, Role = role });
            }
        }

        public async Task<Users> Update(int id, CreateUpdateUserModel model)
        {
            var user = GetQuery().FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                throw new NotFoundException("User is not found!");
            }

            user.EMail = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            AddUserRoles(user, model.Roles);

            await uow.CommitAsync();
            return user;
        }

        public async Task Delete(int id)
        {
            if (!securityContext.IsAdministrator)
                throw new ForbiddenException("Unauthorized operation!");

            var user = GetQuery().FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                throw new NotFoundException("User is not found!");
            }

            if (user.IsDeleted) return;

            user.IsDeleted = true;
            await uow.CommitAsync();
        }

    }
}
