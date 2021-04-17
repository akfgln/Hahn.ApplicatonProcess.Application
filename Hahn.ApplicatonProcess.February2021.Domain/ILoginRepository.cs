using Hahn.ApplicatonProcess.February2021.Data;
using Hahn.ApplicatonProcess.February2021.Domain.Common;
using Hahn.ApplicatonProcess.February2021.Domain.Models;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Domain
{
    public interface ILoginRepository
    {
        UserWithToken Authenticate(string email, string password);
        Task<Users> Register(RegisterLoginModel model);
    }
}
