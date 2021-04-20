using System;
using System.Threading.Tasks;
using FluentAssertions;
using Hahn.ApplicatonProcess.February2021.Domain.Models;
using Hahn.ApplicatonProcess.February2021.IntegrationTests.Common;
using Hahn.ApplicatonProcess.February2021.IntegrationTests.Helpers;
using Xunit;

namespace Hahn.ApplicatonProcess.February2021.IntegrationTests.Login
{
    [Collection("ApiCollection")]
    public class PostShould
    {
        private readonly ApiServer server;
        private readonly HttpClientWrapper client;

        public PostShould(ApiServer server)
        {
            this.server = server;
            client = new HttpClientWrapper(this.server.Client);
        }

        [Fact]
        public async Task AutheticateAdmin()
        {
            var email = "admin@hahn.com";
            var password = "admin";
            var result = await Autheticate(email, password);

            result.User.Email.Should().Be(email);
        }

        public async Task<UserWithTokenModel> Autheticate(string email, string password)
        {
            var response = await client.PostAsync<UserWithTokenModel>("api/Login/Authenticate", new LoginModel
            {
                Email = email,
                Password = password
            });

            return response;
        }
    }
}
