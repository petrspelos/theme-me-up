using Xamarin.Essentials;

namespace ThemeMeUp.Mobile.Services.Implementations
{
    public class UserSettingsService : IUserSettingsService
    {
        public bool LoadFullImageInPreview
        {
            get => Preferences.Get("LoadFullImageInPreview", defaultValue: false);
            set => Preferences.Set("LoadFullImageInPreview", value: value);
        }
    }
}