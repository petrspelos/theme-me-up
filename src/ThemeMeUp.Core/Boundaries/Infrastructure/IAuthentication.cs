namespace ThemeMeUp.Core.Boundaries.Infrastructure
{
    public interface IAuthentication
    {
        string GetApiKey();
        void SetApiKey(string key);
    }
}
