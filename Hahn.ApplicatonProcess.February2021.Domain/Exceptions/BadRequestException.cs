using System;

namespace Hahn.ApplicatonProcess.February2021.Domain.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {
        }
    }
}