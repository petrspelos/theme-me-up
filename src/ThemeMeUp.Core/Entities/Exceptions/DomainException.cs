using System;

namespace ThemeMeUp.Core.Entities.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException() { }
        public DomainException(string message) : base(message) { }
    }
}
