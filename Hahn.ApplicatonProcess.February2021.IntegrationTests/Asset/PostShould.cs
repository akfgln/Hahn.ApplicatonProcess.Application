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
        public async Task<AssetModel> AddNewAsset()
        {
            var requestItem = new AssetModel
            {
                AssetName = "TU_New" + random.Next(),
                CountryOfDepartment = "germany",
                Department = (Departments)(random.Next() % 5),
                EMailAdressOfDepartment = random.Next().ToString()+"@hahn.com",
                PurchaseDate = DateTime.UtcNow
            };

            var createdAsset = await client.PostAsync<AssetModel>("api/Asset", requestItem);

            createdAsset.AssetName.Should().Be(requestItem.AssetName);
            createdAsset.CountryOfDepartment.Should().Be(requestItem.CountryOfDepartment);
            createdAsset.Department.Should().Be(requestItem.Department);
            createdAsset.EMailAdressOfDepartment.Should().Be(requestItem.EMailAdressOfDepartment);
            createdAsset.PurchaseDate.Should().Be(requestItem.PurchaseDate);

            return createdAsset;
        }

    }
}