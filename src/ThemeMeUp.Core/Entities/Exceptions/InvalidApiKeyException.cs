namespace ThemeMeUp.Core.Entities.Exceptions
{
    public class InvalidApiKeyException : DomainException
    {
        public InvalidApiKeyException() { }
        public InvalidApiKeyException(string message) : base(message) { }
    }
}
