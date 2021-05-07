using System.Threading.Tasks;
using ThemeMeUp.Core.Boundaries.Infrastructure;
using ThemeMeUp.Mobile.Models;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ThemeMeUp.Mobile.ViewModels
{
    public class SettingsPageViewModel : BaseViewModel
    {
        private readonly IAuthentication _authentication;

        #region Commands

        public IAsyncCommand SetTokenCommand { get; }

        #endregion

        public SettingsPageViewModel(IAuthentication authentication)
        {
            _authentication = authentication;

            Title = "Settings";
            LoadSettings();

            SetTokenCommand = new AsyncCommand(SetToken, CanExecute);
        }

        private void LoadSettings()
        {
            Token = _authentication.GetApiKey();
            LoadFullImageInPreview = Preferences.Get(SettingKeys.UseFullImagePreviewKey, true);
        }

        private async Task SetToken()
        {
            if (string.IsNullOrWhiteSpace(Token))
            {
                await Application.Current.MainPage.DisplayAlert("No Token found", "Token cannot be empty.", "Okay");
                return;
            }

            _authentication.SetApiKey(Token);

            await Application.Current.MainPage.DisplayAlert("Token added", "Your token was successfully added.", "Okay");
        }

        private bool CanExecute()
        {
            return !IsBusy && !IsRefreshing;
        }

        #region Properties

        private string _token;
        public string Token
        {
            get => _token;
            set
            {
                _token = value;
                OnPropertyChanged();
            }
        }

        private bool _loadFullImageInPreview;
        public bool LoadFullImageInPreview
        {
            get => _loadFullImageInPreview;
            set
            {
                _loadFullImageInPreview = value;
                Preferences.Set(SettingKeys.UseFullImagePreviewKey, value);
                OnPropertyChanged();
            }
        }

        #endregion
    }
}