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
    public class UserRepositoryTest
    {
        private Mock<IUnitOfWork> uow;
        private List<Users> userList;
        private IUserRepository userRepository;
        private Mock<ISecurityContext> securityContext;
        private Random random;
        private List<Roles> roleList;

        public UserRepositoryTest()
        {
            random = new Random();
            uow = new Mock<IUnitOfWork>();
            securityContext = new Mock<ISecurityContext>();
            roleList = new List<Roles>();
            userList = new List<Users>();

            uow.Setup(x => x.Query<Users>()).Returns(() => userList.AsQueryable());
            uow.Setup(x => x.Query<Roles>()).Returns(() => roleList.AsQueryable());
            userRepository = new UserRepository(uow.Object, securityContext.Object);
        }

        [Fact]
        public async Task GetShouldReturnAll()
        {
            userList.Add(new Users());

            var result = userRepository.Get();
            result.Count().Should().Be(1);
        }

        [Fact]
        public async Task GetShouldReturnAllExceptDeleted()
        {
            userList.Add(new Users());
            userList.Add(new Users { IsDeleted = true });
            userList.Add(new Users { IsDeleted = true });

            var result = userRepository.Get();
            result.Count().Should().Be(1);
        }

        [Fact]
        public void GetShouldReturnUserById()
        {
            var user = new Users { Id = random.Next() };
            userList.Add(user);

            var result = userRepository.Get(user.Id);
            result.Should().Be(user);
        }

        [Fact]
        public void GetShouldThrowExceptionIfUserIsNotFoundById()
        {
            var user = new Users { Id = random.Next() };
            userList.Add(user);

            Action get = () =>
            {
                userRepository.Get(random.Next());
            };

            get.Should().Throw<NotFoundException>();
        }

        [Fact]
        public void GetShouldThrowExceptionIfUserIsDeleted()
        {
            var user = new Users { Id = random.Next(), IsDeleted = true };
            userList.Add(user);

            Action get = () =>
            {
                userRepository.Get(user.Id);
            };

            get.Should().Throw<NotFoundException>();
        }

        [Fact]
        public async Task CreateShouldSaveNewUser()
        {
            var model = new CreateUpdateUserModel
            {
                Password = random.Next().ToString(),
                Email = random.Next().ToString(),
                LastName = random.Next().ToString(),
                FirstName = random.Next().ToString(),
            };

            var result = await userRepository.Create(model);

            result.Password.Should().NotBeEmpty();
            result.EMail.Should().Be(model.Email);
            result.LastName.Should().Be(model.LastName);
            result.FirstName.Should().Be(model.FirstName);

            uow.Verify(x => x.Add(result));
            uow.Verify(x => x.CommitAsync());
        }

        [Fact]
        public async Task CreateShouldAddUserRoles()
        {
            var role = new Roles
            {
                DefaultRoleName = random.Next().ToString()
            };
            roleList.Add(role);

            var model = new CreateUpdateUserModel
            {
                Password = random.Next().ToString(),
                Email = random.Next().ToString(),
                LastName = random.Next().ToString(),
                FirstName = random.Next().ToString(),
                Roles = new[] { role.DefaultRoleName }
            };

            securityContext.SetupGet(x => x.IsAdministrator).Returns(true);

            var result = await userRepository.Create(model);

            result.Roles.Should().HaveCount(1);
            result.Roles.Should().Contain(x => x.User == result && x.Role == role);
        }

        [Fact]
        public void CreateShouldThrowExceptionIfEmailIsNotUnique()
        {
            var model = new CreateUpdateUserModel
            {
                Email = random.Next().ToString(),
            };

            userList.Add(new Users { EMail = model.Email });

            Action create = () =>
            {
                var x = userRepository.Create(model).Result;
            };

            create.Should().Throw<BadRequestException>();
        }

        [Fact]
        public async Task UpdateShouldUpdateUserFields()
        {
            var user = new Users { Id = random.Next() };
            userList.Add(user);

            var model = new CreateUpdateUserModel
            {
                Email = random.Next().ToString(),
                LastName = random.Next().ToString(),
                FirstName = random.Next().ToString(),
            };

            var result = await userRepository.Update(user.Id, model);

            result.Should().Be(user);
            result.EMail.Should().Be(model.Email);
            result.LastName.Should().Be(model.LastName);
            result.FirstName.Should().Be(model.FirstName);

            uow.Verify(x => x.CommitAsync());
        }

        [Fact]
        public async Task UpdateShouldUpdateRoles()
        {
            var role = new Roles
            {
                DefaultRoleName = random.Next().ToString()
            };
            roleList.Add(role);

            var user = new Users
            {
                Id = random.Next(),
                Roles = new List<UserRoles>
                {
                    new UserRoles()
                }
            };
            userList.Add(user);

            var model = new CreateUpdateUserModel
            {
                LastName = random.Next().ToString(),
                FirstName = random.Next().ToString(),
                Roles = new[] { role.DefaultRoleName }
            };

            securityContext.SetupGet(x => x.IsAdministrator).Returns(true);

            var result = await userRepository.Update(user.Id, model);

            result.Roles.Should().HaveCount(1);
            result.Roles.Should().Contain(x => x.User == result && x.Role == role);
        }

        [Fact]
        public void UpdateShoudlThrowExceptionIfUserIsNotFound()
        {
            Action create = () =>
            {
                var result = userRepository.Update(random.Next(), new CreateUpdateUserModel()).Result;
            };

            create.Should().Throw<NotFoundException>();
        }

        [Fact]
        public async Task DeleteShouldMarkUserAsDeleted()
        {
            var user = new Users { Id = random.Next() };
            userList.Add(user);

            securityContext.SetupGet(x => x.IsAdministrator).Returns(true);

            await userRepository.Delete(user.Id);

            user.IsDeleted.Should().BeTrue();

            uow.Verify(x => x.CommitAsync());
        }

        [Fact]
        public void DeleteShoudlThrowExceptionIfUserIsNotFound()
        {
            securityContext.SetupGet(x => x.IsAdministrator).Returns(true);
            Action create = () =>
            {
                userRepository.Delete(random.Next()).Wait();
            };

            create.Should().Throw<NotFoundException>();
        }
    }
}
