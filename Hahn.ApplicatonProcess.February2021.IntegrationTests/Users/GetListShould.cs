using System.Net.Http;
using System.Threading.Tasks;
using Hahn.ApplicatonProcess.February2021.IntegrationTests.Common;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;
using Hahn.ApplicatonProcess.February2021.Domain.Models;
using Hahn.ApplicatonProcess.February2021.Domain.Common;

namespace Hahn.ApplicatonProcess.February2021.IntegrationTests.Users
{
    [Collection("ApiCollection")]
    public class GetListShould
    {
        private readonly ApiServer server;
        private readonly HttpClient client;

        public GetListShould(ApiServer server)
        {
            this.server = server;
            client = this.server.Client;
        }

        public static async Task<DataResult<UserModel>> Get(HttpClient client)
        {
            var response = await client.GetAsync($"api/Users?skip={0}&take={1}&wrapwith=count,total-count,next-link");
            response.EnsureSuccessStatusCode();
            var responseText = await response.Content.ReadAsStringAsync();
            var items = JsonConvert.DeserializeObject<DataResult<UserModel>>(responseText);
            return items;
        }

        [Fact]
        public async Task ReturnAnyList()
        {
            var items = await Get(client);
            items.Should().NotBeNull();
            items.Result.Should().NotBeEmpty();
        }
    }
}