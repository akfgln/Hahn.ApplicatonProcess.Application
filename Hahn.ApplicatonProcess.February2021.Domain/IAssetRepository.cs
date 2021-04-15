using Hahn.ApplicatonProcess.February2021.Data;
using Hahn.ApplicatonProcess.February2021.Domain.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Domain
{
    public interface IAssetRepository
    {
        IQueryable<Asset> Get();
        Asset Get(int id);
        Task<Asset> Create(AssetModel model);
        Task<Asset> Update(int id, UpdateAssetModel model);
        Task Delete(int id);
    }
}
