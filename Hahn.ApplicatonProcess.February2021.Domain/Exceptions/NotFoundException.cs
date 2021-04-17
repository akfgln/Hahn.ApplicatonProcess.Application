using System;

namespace Hahn.ApplicatonProcess.February2021.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }
}