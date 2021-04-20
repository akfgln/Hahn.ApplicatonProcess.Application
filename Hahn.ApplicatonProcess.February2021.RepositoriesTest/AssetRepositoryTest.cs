using FluentAssertions;
using Hahn.ApplicatonProcess.February2021.Data;
using Hahn.ApplicatonProcess.February2021.Domain;
using Hahn.ApplicatonProcess.February2021.Domain.Exceptions;
using Hahn.ApplicatonProcess.February2021.Domain.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Hahn.ApplicatonProcess.February2021.RepositoriesTest
{
    public class AssetRepositoryTest
    {
        private Mock<IUnitOfWork> uow;
        private List<Asset> assetList;
        private IAssetRepository assetRepository;
        private Random random;
        public AssetRepositoryTest()
        {
            random = new Random();
            uow = new Mock<IUnitOfWork>();
            assetList = new List<Asset>();
            uow.Setup(x => x.Query<Asset>()).Returns(() => assetList.AsQueryable());
            assetRepository = new AssetRepository(uow.Object);
        }
        [Fact]
        public void GetShouldReturnAll()
        {
            assetList.Add(new Asset { AssetName = random.Next().ToString() });

            var result = assetRepository.Get().ToList();
            result.Count().Should().Be(1);
        }

        [Fact]
        public void GetShouldReturnAllExceptDeleted()
        {
            assetList.Add(new Asset { AssetName = random.Next().ToString() });
            assetList.Add(new Asset { AssetName = random.Next().ToString(), IsDeleted = true });

            var result = assetRepository.Get();
            result.Count().Should().Be(1);
        }

        [Fact]
        public void GetShouldReturnById()
        {
            var asset = new Asset { Id = random.Next(), AssetName = random.Next().ToString() };
            assetList.Add(asset);

            var result = assetRepository.Get(asset.Id);
            result.Should().Be(asset);
        }

        [Fact]
        public void GetShouldThrowExceptionIfItemIsNotFoundById()
        {
            var asset = new Asset { Id = random.Next(), AssetName = random.Next().ToString() };
            assetList.Add(asset);

            Action get = () =>
            {
                assetRepository.Get(random.Next());
            };

            get.Should().Throw<NotFoundException>();
        }

        [Fact]
        public async Task CreateShouldSaveNew()
        {
            var model = new AssetModel
            {
                AssetName = random.Next().ToString(),
                CountryOfDepartment = random.Next().ToString(),
                Department = Domain.Models.Departments.HQ,
                EMailAdressOfDepartment = random.Next().ToString(),
                PurchaseDate = DateTime.UtcNow
            };

            var result = await assetRepository.Create(model);

            result.AssetName.Should().Be(model.AssetName);
            result.CountryOfDepartment.Should().Be(model.CountryOfDepartment);
            result.Department.Should().Be(model.Department.ToString());
            result.EMailAdressOfDepartment.Should().Be(model.EMailAdressOfDepartment);
            result.PurchaseDate.Should().BeCloseTo(model.PurchaseDate);

            uow.Verify(x => x.Add(result));
            uow.Verify(x => x.CommitAsync());
        }

        [Fact]
        public async Task UpdateShouldUpdateFields()
        {
            var asset = new Asset
            {
                Id = random.Next(),
                AssetName = random.Next().ToString(),
                CountryOfDepartment = random.Next().ToString(),
                Department = Domain.Models.Departments.Store1.ToString(),
                PurchaseDate = DateTime.UtcNow
            };
            assetList.Add(asset);

            var model = new AssetModel
            {
                Id = asset.Id,
                CountryOfDepartment = random.Next().ToString(),
                Department = Domain.Models.Departments.MaintenanceStation,
            };

            var result = await assetRepository.Update(asset.Id, model);

            result.Should().Be(asset);
            result.AssetName.Should().Be(asset.AssetName);
            result.CountryOfDepartment.Should().Be(model.CountryOfDepartment);
            result.Department.Should().Be(model.Department.ToString());
            result.PurchaseDate.Should().BeCloseTo(asset.PurchaseDate);

            uow.Verify(x => x.CommitAsync());
        }

        [Fact]
        public void UpdateShouldThrowExceptionIfItemIsNotFound()
        {
            Action create = () =>
            {
                var result = assetRepository.Update(random.Next(), new AssetModel { Id = random.Next() }).Result;
            };

            create.Should().Throw<NotFoundException>();
        }

        [Fact]
        public async Task DeleteShouldMarkAsDeleted()
        {
            var asset = new Asset() { Id = random.Next(), AssetName = random.Next().ToString() };
            assetList.Add(asset);

            await assetRepository.Delete(asset.Id);

            asset.IsDeleted.Should().BeTrue();

            uow.Verify(x => x.CommitAsync());
        }

        [Fact]
        public void DeleteShouldThrowExceptionIfItemIsNotFound()
        {
            Action execute = () =>
            {
                assetRepository.Delete(random.Next()).Wait();
            };

            execute.Should().Throw<NotFoundException>();
        }
    }
}
