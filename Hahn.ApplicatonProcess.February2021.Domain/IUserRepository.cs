using Hahn.ApplicatonProcess.February2021.Data;
using Hahn.ApplicatonProcess.February2021.Domain.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Domain
{
    public interface IUserRepository
    {
        IQueryable<Users> Get();
        Users Get(int id);
        Task<Users> Create(CreateUserModel model);
        Task<Users> Update(int id, UpdateUserModel model);
        Task Delete(int id);
    }
}
