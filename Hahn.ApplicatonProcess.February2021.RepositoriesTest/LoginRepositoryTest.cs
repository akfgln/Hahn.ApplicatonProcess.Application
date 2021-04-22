using FluentAssertions;
using Hahn.ApplicatonProcess.February2021.Data;
using Hahn.ApplicatonProcess.February2021.Data.Helpers;
using Hahn.ApplicatonProcess.February2021.Domain;
using Hahn.ApplicatonProcess.February2021.Domain.Common;
using Hahn.ApplicatonProcess.February2021.Domain.Exceptions;
using Hahn.ApplicatonProcess.February2021.Domain.Models;
using Hahn.ApplicatonProcess.February2021.Domain.Security;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Hahn.ApplicatonProcess.February2021.RepositoriesTest
{
    public class LoginRepositoryTest
    {
        private Mock<IUnitOfWork> uow;
        private List<Users> userList;
        private ILoginRepository loginRepository;
        private Random random;
        private Mock<IPermissionContext> securityContext;
        private Mock<ITokenBuilder> tokenBuilder;
        private Mock<IUserRepository> userRepository;

        public LoginRepositoryTest()
        {
            random = new Random();
            uow = new Mock<IUnitOfWork>();

            userList = new List<Users>();
            uow.Setup(x => x.Query<Users>()).Returns(() => userList.AsQueryable());

            tokenBuilder = new Mock<ITokenBuilder>(MockBehavior.Strict);
            userRepository = new Mock<IUserRepository>();
            securityContext = new Mock<IPermissionContext>(MockBehavior.Strict);

            loginRepository = new LoginRepository(uow.Object, tokenBuilder.Object, userRepository.Object, securityContext.Object);
        }

        [Fact]
        public void AuthenticateShouldReturnUserAndToken()
        {
            var password = random.Next().ToString();
            var user = new Users
            {
                EMail = random.Next().ToString(),
                Password = password.WithBCrypt(),
                Roles = new List<UserRoles>
                {
                    new UserRoles{Role = new Roles {DefaultRoleName = random.Next().ToString()}},
                    new UserRoles{Role = new Roles {DefaultRoleName = random.Next().ToString()}},
                }
            };
            userList.Add(user);

            var expireTokenDate = DateTime.UtcNow + TokenAuthOption.ExpiresSpan;

            var token = random.Next().ToString();
            tokenBuilder.Setup(tb => tb.Build(

                user.EMail,
                It.Is<string[]>(roles => roles.SequenceEqual(user.Roles.Select(x => x.Role.DefaultRoleName).ToArray())),

                It.Is<DateTime>(d => d - expireTokenDate < TimeSpan.FromSeconds(1))))
                .Returns(token);

            var result = loginRepository.Authenticate(user.EMail, password);

            result.User.Should().Be(user);
            result.Token.Should().Be("Bearer "+ token);
            result.Expiration.Should().BeCloseTo(expireTokenDate, 1000);
        }

        [Fact]
        public void AuthenticateShouldThrowIfUserPasswordIsWrong()
        {
            var password = random.Next().ToString();
            var user = new Users
            {
                EMail = random.Next().ToString(),
                Password = password.WithBCrypt(),
            };
            userList.Add(user);

            Action execute = () => loginRepository.Authenticate(user.EMail, random.Next().ToString());

            execute.Should().Throw<BadRequestException>();
        }

        [Fact]
        public void AuthenticateShouldThrowIfUserIsDeleted()
        {
            var password = random.Next().ToString();
            var user = new Users
            {
                EMail = random.Next().ToString(),
                Password = password.WithBCrypt(),
                IsDeleted = true
            };
            userList.Add(user);

            Action execute = () => loginRepository.Authenticate(user.EMail, password);

            execute.Should().Throw<BadRequestException>();
        }

        [Fact]
        public async Task RegisterShouldCreateUserViaQuery()
        {
            var requestModel = new RegisterLoginModel
            {
                Password = random.Next().ToString(),
                Email = random.Next().ToString(),
                LastName = random.Next().ToString(),
                FirstName = random.Next().ToString(),
            };

            var createdUser = new Users();

            userRepository.Setup(x => x.Create(It.Is<CreateUserModel>(m =>
                m.FirstName == requestModel.FirstName
                && m.LastName == requestModel.LastName
                && m.Password == requestModel.Password
                && m.Email == requestModel.Email
                && m.Roles.Length == 0
            ))).Returns(Task.FromResult(createdUser));

            var result = await loginRepository.Register(requestModel);

            result.Should().Be(createdUser);
        }
    }
}
