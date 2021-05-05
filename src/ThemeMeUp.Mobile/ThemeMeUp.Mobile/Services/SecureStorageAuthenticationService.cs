using System;
using ThemeMeUp.Core.Boundaries.Infrastructure;
using Xamarin.Essentials;

namespace ThemeMeUp.Mobile.Services
{
    public class SecureStorageAuthenticationService : IAuthentication
    {
        public string GetApiKey()
        {
            try
            {
                var oauthToken = SecureStorage.GetAsync("oauth_token");
                return oauthToken.Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public void SetApiKey(string key)
        {
            try
            {
                SecureStorage.SetAsync("oauth_token", key);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}