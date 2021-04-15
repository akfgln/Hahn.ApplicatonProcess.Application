using Hahn.ApplicatonProcess.February2021.Data;
using Hahn.ApplicatonProcess.February2021.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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

      
    }
}
