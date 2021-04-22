using Hahn.ApplicatonProcess.February2021.Data;

namespace Hahn.ApplicatonProcess.February2021.Domain.Security
{
    public interface IPermissionContext
    {
        Users User { get; }
        bool IsAdministrator { get; }
    }
}
