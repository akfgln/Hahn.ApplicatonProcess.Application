using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Hahn.ApplicatonProcess.February2021.IntegrationTests.Common;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;
using Hahn.ApplicatonProcess.February2021.Domain.Models;

namespace Hahn.ApplicatonProcess.February2021.IntegrationTests.Users
{
    [Collection("ApiCollection")]
    public class GetItemShould
    {
        private readonly ApiServer server;
        private readonly HttpClient client;

        public GetItemShould(ApiServer server)
        {
            this.server = server;
            client = this.server.Client;
        }

        [Fact]
        public async Task ReturnItemById()
        {
            var item = await new PostShould(server).RegisterNewUser();

            var result = await GetById(client, item.Id);

            result.Should().NotBeNull();
        }

        public static async Task<UserModel> GetById(HttpClient client, int id)
        {
            var response = await client.GetAsync(new Uri($"api/Users/{id}", UriKind.Relative));
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserModel>(result);
        }

        [Fact]
        public async Task ShouldReturn404StatusIfNotFound()
        {
            var response = await client.GetAsync(new Uri($"api/Users/-1", UriKind.Relative));
            
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}