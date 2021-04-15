using System;

namespace Hahn.ApplicatonProcess.February2021.Data
{
    public interface ITransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}
