using System;
using System.Net.Http;
using System.Threading.Tasks;
using Hahn.ApplicatonProcess.February2021.IntegrationTests.Common;
using Xunit;

namespace Hahn.ApplicatonProcess.February2021.IntegrationTests.Asset
{
    [Collection("ApiCollection")]
    public class DeleteShould
    {
        private readonly ApiServer server;
        private readonly HttpClient client;

        public DeleteShould(ApiServer server)
        {
            this.server = server;
            client = server.Client;
        }

        [Fact]
        public async Task DeleteExistingItem()
        {
            var item = await new PostShould(server).AddNewAsset();

            var response = await client.DeleteAsync(new Uri($"api/Asset/{item.Id}", UriKind.Relative));
            response.EnsureSuccessStatusCode();
        }
    }
}