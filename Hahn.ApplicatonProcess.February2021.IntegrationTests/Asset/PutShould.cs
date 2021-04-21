using System;
using System.Threading.Tasks;
using Hahn.ApplicatonProcess.February2021.IntegrationTests.Common;
using Hahn.ApplicatonProcess.February2021.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;
using Hahn.ApplicatonProcess.February2021.Domain.Models;

namespace Hahn.ApplicatonProcess.February2021.IntegrationTests.Asset
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
            var item = await new PostShould(server).AddNewAsset();

            var requestItem = new AssetModel
            {
                AssetName = "TU_Update_" + random.Next(),
                CountryOfDepartment = "United Kingdom of Great Britain and Northern Ireland",
                Department = (Departments)(random.Next() % 5),
                EMailAdressOfDepartment = "TU_Update_" + random.Next().ToString() + "@hahn.com",
                PurchaseDate = DateTime.UtcNow
            };

            await client.PutAsync<AssetModel>($"api/Asset/{item.Id}", requestItem);

            var updatedAsset = await GetItemShould.GetById(client.Client, item.Id);
            updatedAsset.AssetName.Should().Be(requestItem.AssetName);
            updatedAsset.CountryOfDepartment.Should().Be(requestItem.CountryOfDepartment);
            updatedAsset.Department.Should().Be(requestItem.Department);
            updatedAsset.EMailAdressOfDepartment.Should().Be(requestItem.EMailAdressOfDepartment);
        }
    }
}