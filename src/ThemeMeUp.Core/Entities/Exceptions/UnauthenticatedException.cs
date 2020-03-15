namespace ThemeMeUp.Core.Entities.Exceptions
{
    public class UnauthenticatedException : DomainException
    {
        public UnauthenticatedException() { }
        public UnauthenticatedException(string message) : base(message) { }
    }
}
