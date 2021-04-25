using Hahn.ApplicatonProcess.February2021.Data;
using Hahn.ApplicatonProcess.February2021.Domain.Exceptions;
using Hahn.ApplicatonProcess.February2021.Domain.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Domain
{
    public class AssetRepository : IAssetRepository
    {

        private readonly IUnitOfWork _uow;


        public AssetRepository(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IQueryable<Asset> Get()
        {
            var query = GetQuery();
            return query;
        }

        private IQueryable<Asset> GetQuery()
        {
            var q = _uow.Query<Asset>().Where(x => !x.IsDeleted);
            return q;
        }

        public Asset Get(int id)
        {
            var asset = GetQuery().FirstOrDefault(x => x.Id == id);

            if (asset == null)
            {
                throw new NotFoundException(message: "Asset is not found!");
            }
            return asset;
        }

        public async Task<Asset> Create(AssetModel model)
        {
            var item = new Asset
            {
                AssetName = model.AssetName,
                CountryOfDepartment = model.CountryOfDepartment,
                Department = model.Department.ToString(),
                EMailAdressOfDepartment = model.EMailAdressOfDepartment,
                IsBroken = model.IsBroken,
                PurchaseDate = model.PurchaseDate
            };

            _uow.Add(item);
            await _uow.CommitAsync();

            return item;
        }

        public async Task<Asset> Update(int id, AssetModel model)
        {
            var asset = GetQuery().FirstOrDefault(x => x.Id == id);

            if (asset == null)
            {
                throw new NotFoundException("Asset is not found");
            }

            asset.AssetName = model.AssetName;
            asset.CountryOfDepartment = model.CountryOfDepartment;
            asset.Department = model.Department.ToString();
            asset.EMailAdressOfDepartment = model.EMailAdressOfDepartment;
            asset.IsBroken = model.IsBroken;
            asset.PurchaseDate = model.PurchaseDate;

            await _uow.CommitAsync();
            return asset;
        }

        public async Task Delete(int id)
        {
            var asset = GetQuery().FirstOrDefault(u => u.Id == id);

            if (asset == null)
            {
                throw new NotFoundException("Asset is not found");
            }

            if (asset.IsDeleted) return;

            asset.IsDeleted = true;
            await _uow.CommitAsync();
        }
    }
}
