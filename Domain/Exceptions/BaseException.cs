using System;
namespace Domain.Exceptions
{
    public class BaseException : Exception
    {
        protected BaseException(string message) : base(message)
        { }

    }
}

