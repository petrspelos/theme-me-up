using System;
using ThemeMeUp.Core.Boundaries.Infrastructure;

namespace ThemeMeUp.Infrastructure
{
    public class EnvironmentAuthentication : IAuthentication
    {
        public string GetApiKey() => Environment.GetEnvironmentVariable("WALLHAVEN_API_KEY");
    }
}
