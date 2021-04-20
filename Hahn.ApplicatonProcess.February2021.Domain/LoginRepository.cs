using Hahn.ApplicatonProcess.February2021.Data;
using Hahn.ApplicatonProcess.February2021.Domain.Common;
using Hahn.ApplicatonProcess.February2021.Domain.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Hahn.ApplicatonProcess.February2021.Data.Helpers;
using Hahn.ApplicatonProcess.February2021.Domain.Security;
using Hahn.ApplicatonProcess.February2021.Domain.Exceptions;

namespace Hahn.ApplicatonProcess.February2021.Domain
{
    public class LoginRepository : ILoginRepository
    {
        private readonly IUnitOfWork uow;
        private readonly ITokenBuilder tokenBuilder;
        private readonly IUserRepository userRepository;
        private readonly ISecurityContext securityContext;

        public LoginRepository(IUnitOfWork uow, ITokenBuilder tokenBuilder, IUserRepository userRepository, ISecurityContext securityContext)
        {
            this.uow = uow;
            this.tokenBuilder = tokenBuilder;
            this.userRepository = userRepository;
            this.securityContext = securityContext;
        }

        public UserWithToken Authenticate(string email, string password)
        {
            var user = (from u in uow.Query<Users>()
                        where u.EMail == email && !u.IsDeleted
                        select u)
                .Include(x => x.Roles)
                .ThenInclude(x => x.Role)
                .FirstOrDefault();

            if (user == null)
            {
                throw new BadRequestException("email/password aren't right");
            }

            if (string.IsNullOrWhiteSpace(password) || !user.Password.VerifyWithBCrypt(password))
            {
                throw new BadRequestException("email/password aren't right");
            }

            var expiresIn = DateTime.UtcNow + TokenAuthOption.ExpiresSpan;
            var token = this.tokenBuilder.Build(user.EMail, user.Roles.Select(x => x.Role.DefaultRoleName).ToArray(), expiresIn);

            return new UserWithToken
            {
                ExpiresAt = expiresIn,
                Expiration = expiresIn,
                Token = token,
                User = user
            };
        }

        public async Task<Users> Register(RegisterLoginModel model)
        {
            var requestModel = new CreateUserModel
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = model.Password,
                Email = model.Email
            };

            var user = await this.userRepository.Create(requestModel);
            return user;
        }
    }
}
