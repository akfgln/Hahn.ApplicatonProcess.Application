using System;
using System.Threading.Tasks;
using Hahn.ApplicatonProcess.February2021.IntegrationTests.Common;
using Hahn.ApplicatonProcess.February2021.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;
using Hahn.ApplicatonProcess.February2021.Domain.Models;

namespace Hahn.ApplicatonProcess.February2021.IntegrationTests.Users
{
    [Collection("ApiCollection")]
    public class PostShould
    {
        private readonly ApiServer server;
        private readonly HttpClientWrapper client;
        private Random random;

        public PostShould(ApiServer server)
        {
            random = new Random();
            this.server = server;
            client = new HttpClientWrapper(this.server.Client);
        }

        [Fact]
        public async Task<UserModel> RegisterNewUser()
        {
            var requestItem = new RegisterLoginModel
            {
                Email = "TU_" + random.Next(),
                Password = random.Next().ToString(),
                LastName = random.Next().ToString(),
                FirstName = random.Next().ToString()
            };

            var createdUser = await client.PostAsync<UserModel>("api/Login/Register", requestItem);

            createdUser.Roles.Should().BeEmpty();
            createdUser.Email.Should().Be(requestItem.Email);
            createdUser.LastName.Should().Be(requestItem.LastName);
            createdUser.FirstName.Should().Be(requestItem.FirstName);

            return createdUser;
        }

    }
}