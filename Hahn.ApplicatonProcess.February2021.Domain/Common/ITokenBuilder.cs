using System;

namespace Hahn.ApplicatonProcess.February2021.Domain.Common
{
    public interface ITokenBuilder
    {
        string Build(string name, string[] roles, DateTime expireDate);
    }
}
