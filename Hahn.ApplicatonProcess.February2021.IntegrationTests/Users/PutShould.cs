using System;
using System.Threading.Tasks;
using Hahn.ApplicatonProcess.February2021.IntegrationTests.Common;
using Hahn.ApplicatonProcess.February2021.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;
using Hahn.ApplicatonProcess.February2021.Domain.Models;
using Hahn.ApplicatonProcess.February2021.Domain.Common;

namespace Hahn.ApplicatonProcess.February2021.IntegrationTests.Users
{
    [Collection("ApiCollection")]
    public class PutShould
    {
        private readonly ApiServer server;
        private readonly HttpClientWrapper client;
        private Random random;

        public PutShould(ApiServer server)
        {
            this.server = server;
            client = new HttpClientWrapper(this.server.Client);
            random = new Random();
        }

        [Fact]
        public async Task UpdateExistingItem()
        {
            var item = await new PostShould(server).RegisterNewUser();

            var requestItem = new UpdateUserModel
            {
                FirstName = "TU_Update_" + random.Next().ToString(),
                LastName = "TU_Update_" + random.Next().ToString(),
                Roles = new[] {SystemRoles.Manager}
            };

            await client.PutAsync<UserModel>($"api/Users/{item.Id}", requestItem);

            var updatedUser = await GetItemShould.GetById(client.Client, item.Id);

            updatedUser.FirstName.Should().Be(requestItem.FirstName);
            updatedUser.LastName.Should().Be(requestItem.LastName);

            updatedUser.Roles.Should().HaveCount(1);
            updatedUser.Roles.Should().Contain(SystemRoles.Manager);
        }
    }
}