using System;

namespace Hahn.ApplicatonProcess.February2021.Domain.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message) : base(message)
        {
        }
    }
}