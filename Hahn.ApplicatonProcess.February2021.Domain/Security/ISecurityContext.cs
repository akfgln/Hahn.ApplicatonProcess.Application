using Hahn.ApplicatonProcess.February2021.Data;

namespace Hahn.ApplicatonProcess.February2021.Domain.Security
{
    public interface ISecurityContext
    {
        Users User { get; }
        bool IsAdministrator { get; }
    }
}
