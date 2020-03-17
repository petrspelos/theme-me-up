using System;
using ThemeMeUp.Core.Boundaries.Infrastructure;

namespace ThemeMeUp.Infrastructure
{
    public class EnvironmentAuthentication : IAuthentication
    {
        public string GetApiKey() => Environment.GetEnvironmentVariable("WALLHAVEN_API_KEY", EnvironmentVariableTarget.User);
        public void SetApiKey(string key) => Environment.SetEnvironmentVariable("WALLHAVEN_API_KEY", key, EnvironmentVariableTarget.User);
    }
}
